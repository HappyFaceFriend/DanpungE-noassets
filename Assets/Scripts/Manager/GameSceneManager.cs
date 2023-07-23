using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class GameSceneManager:HappyTools.SingletonBehaviour<GameSceneManager>
{
    [SerializeField] InGameUISet _uiSet;

    //각 판이 시작될때마다 호출되어야 함.   
    public MapType Map = MapType.Farm;
    public Player Player { get; private set; }
    private int _holdingItemAmount;

    [SerializeField] public int MinItemSellAmount = 10; //10개 붂음
    [SerializeField] public int ItemSellPrice = 20;
    
    
    public UnityEvent onObtainItem;

    [SerializeField] Stage _farmStage;
    [SerializeField] Stage _fishStage;
    [SerializeField] CameraController _camera;

    [SerializeField] Vector3 _farmPlayerSpawnOffset = new Vector3(-26, 15, 0);
    [SerializeField] float _playerSpawnDuration = 1f;


    [SerializeField] MotionBlurEffect _motionBlurEffect;
    [Header("Durations")]
    [SerializeField] float _windDuration;
    [SerializeField] float _dropDuration;
    [SerializeField] int InitialWindCoolTime; //60초
    [SerializeField] int WindCoolTime; //75초

    [SerializeField] Vector3 _windToLeft;
    [SerializeField] Vector3 _windToRight;
    [SerializeField] WindEffectManager _windEffectManager;

    [SerializeField] int _maxCoinGain = 5;
    int _stageStartCoin = 0;

    Stage _currentStage;

    Coroutine _currentTimer;
    private void Awake()
    {
        base.Awake();
        _currentStage = _farmStage;
        Debug.LogWarning("TESTCODE : UPDATE");
    }
    private void Start()
    {
        Init();
        SoundManager.Instance.PlayCommonBGM("AfterTutBGM");
        SoundManager.Instance.StopBGM2();
    }
    private void Update()
    {
        //디버깅용
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            StartCoroutine(StageChangeCoroutine());
        }
    }
    public void DecrementItem(int idx)
    {
        _currentStage.ItemSpawner.Squares[idx]--;
    }
    
    //마을 전환 함수
    IEnumerator StageChangeCoroutine(bool initial = false)
    {
        _stageStartCoin = GameManager.Instance.GetGold();
        StartCoroutine(WindTimer(initial));
        MapType nextType = Map == MapType.Farm ? MapType.FishingVillage : MapType.Farm;

        Stage nextStage = Map == MapType.FishingVillage ? _farmStage : _fishStage;

        _currentStage.StopChasing();
        
        _currentStage.RemoveWalls();
        Player.OnStageChangeStart(initial);
        _camera.FollowTarget = null;

        if (!initial)
        {
            //wind
            if (nextType == MapType.Farm)
            {
                _windEffectManager.transform.localScale = Vector3.one;
                _windEffectManager.transform.localPosition = Vector3.zero;
            }
            else
            {
                _windEffectManager.transform.localScale = new Vector3(-1, 1, 1);
                _windEffectManager.transform.localPosition = new Vector3(7,2.3f,0);
            }
            _windEffectManager.gameObject.SetActive(true);
            float eTime = 0f;
            while(eTime < _windDuration)
            {
                eTime += Time.deltaTime;
                yield return null;
                if (nextType == MapType.Farm)
                    Player.transform.position += _windToLeft * Time.deltaTime;
                else
                    Player.transform.position += _windToRight * Time.deltaTime;
            }
            _currentStage.SpawnWalls();
            _windEffectManager.gameObject.SetActive(false);
        }
        else
        {
            nextStage = _farmStage;
            nextType = MapType.Farm;
            _currentStage.SpawnWalls();
            _camera.transform.position = new Vector3(-300,0,300);
            Player.transform.position = new Vector3(0, 100, 0);
        }


        nextStage.ItemSpawner.StartSpawning();
        nextStage.ResetChiefPosition(); 
        yield return _motionBlurEffect.SceneChangeCoroutine(nextType);

        if(!initial)
            _currentStage.ItemSpawner.ResetCollectables();
        Map = nextType;
        _currentStage = nextStage;
        /*
        if (Map == MapType.Farm)
            SoundManager.Instance.PlayCommonBGM("FarmBGM");
        else
            SoundManager.Instance.PlayCommonBGM("FishBGM");*/
        yield return SpawnPlayerCoroutine(_dropDuration);
        Player.OnStageChangeOver();
        _currentStage.StartChasing();
    }

    void ForceFly()
    {
        _uiSet.OnForceFly();
        StartCoroutine(StageChangeCoroutine());
    }
    IEnumerator WindTimer(bool initial = false)
    {
        bool timerStarted = false;
        float deltaTime = 0f;
        float maxTime = initial?InitialWindCoolTime : WindCoolTime;
        while (deltaTime < maxTime)
        {
            //일시정지 처리
            if (GameManager.Instance.IsGamePause)
            {
                yield return new WaitUntil(() => GameManager.Instance.IsGamePause == false);
            }

            if (deltaTime > maxTime - 10 && !timerStarted)
            {
                timerStarted = true;
                _uiSet.SetWindTimer();
            }

            if (_maxCoinGain <= GameManager.Instance.GetGold() - _stageStartCoin)
            {
                ForceFly();
                yield break;
            }
            deltaTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(StageChangeCoroutine());
    }
    public override void Init()
    {
        //farm과 fishingvillage 전환
        Debug.Log("GameSceneManager - Init");
        Player = GameObject.Find("GameObjects").transform.Find("Player").GetComponent<Player>();
        Player.Init();
        _holdingItemAmount = 0;
        _camera.FollowTarget = Player.transform;
        StartCoroutine(StartAnimation());
    }

    IEnumerator SpawnPlayerCoroutine(float duration, bool isInitial = false)
    {
        Player.IsControlledByOther = true;
        Player.Rigidbody.velocity = Vector3.zero;
        Player.Rigidbody.isKinematic = true;
        _camera.FollowTarget = null;

        Vector3 startPosition = _currentStage.transform.position;
        if (Map == MapType.FishingVillage && !isInitial)
            startPosition += Vector3.Scale(_farmPlayerSpawnOffset, new Vector3(-1, 1, -1));
        else
            startPosition += _farmPlayerSpawnOffset;
    
            Player.transform.position = startPosition;
        Vector3 targetPosition = _currentStage.transform.position + new Vector3(0, Player.GetComponent<BoxCollider>().size.y / 2, 0);
        float eTime = 0f;
        while(eTime < duration)
        {
            eTime += Time.deltaTime;
            yield return null;
            float t = eTime / duration;
            Player.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        }
        Player.transform.position = targetPosition;
        Player.Rigidbody.isKinematic = false;
        Player.Rigidbody.velocity = Vector3.zero;
        Player.IsControlledByOther = false;
        _camera.FollowTarget = Player.transform;
    }

    public bool IsOpeningAnimationFinished = false;
    IEnumerator StartAnimation()
    {
        IsOpeningAnimationFinished = false;
        /*
        switch (Map)
        {
            case MapType.Farm:
                //농촌 시작화면
                break;
            case MapType.FishingVillage:
                //어촌 시작화면
                break;
        }*/

        yield return StageChangeCoroutine(true);

        IsOpeningAnimationFinished = true;
        Debug.Log("GameSceneManager - Opening Animation Finished");
        yield return null;
    }

    public void RandomItemGenerate()
    {
        //TODO: 랜덤으로 아이템 생성
    }

    public void OnObtainItem()
    {
        _holdingItemAmount ++;
        _uiSet.OnObtainItem();
    }
    public int GetHoldingItemAmount()
    {
        return _holdingItemAmount;
    }

    public void SetHoldingItemAmount(int amount)
    {
        _holdingItemAmount = amount;
    }

    public void SetItemIcon(MapType nextType)
    {
        bool isFarm = nextType == MapType.Farm;
        _uiSet.SetItemIcon(isFarm);
    }

    public void UpdateText()
    {
        _uiSet.UpdateText();
    }

    public void EndGame()
    {
        StopAllCoroutines();
    }
}