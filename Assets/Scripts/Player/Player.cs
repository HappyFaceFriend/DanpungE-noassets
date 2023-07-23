using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerDataSetting _playerDataSetting;
    [SerializeField] private SpriteRenderer _playerSprite;
    [SerializeField] private GameObject _playerDashObject;
    [SerializeField] private GameObject _dashChargeEffect;
    [SerializeField] private ItemLoseEffect itemLoseEffect;
    [SerializeField] private SpriteRenderer itemLoseImage;
    private bool isMoving = false;
    private bool isWalking = false;
    private bool isDashing = false;
    private bool isDashCoolDown = false;
    private bool isFlipX = false;
    private int _speed;
    private int _dashSpeed;
    private float _dashDuration;
    private float _dashEffectCount;
    private float _playerDashFadeTime;
    private float _dashCoolTime;
    private float _invincibleTime;
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody { get { return _rigidbody; } }

    Collider _collider;
    public bool IsControlledByOther { get; set; } = true;

    private Vector3 _lastDir = Vector3.zero;
    private int dashCounter;
    public bool IsInvincible = false;

    bool _isHitted = false;
    
    public Animator aim;
    // Start is called before the first frame update

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        isFlipX = true;
        _dashChargeEffect.gameObject.SetActive(false);
        SetPlayerFlipX();
    }
    private void SetPlayerFlipX()
    {
        Vector3 scale = _playerSprite.transform.localScale;
        int x = isFlipX ? -1 : 1;
        _playerSprite.transform.localScale = new Vector3(x, scale.y, scale.z);
    }
    void Start()
    {
        Init();
    }

    public void Init()
    {
        Debug.Log("Player.Init");
        _speed = _playerDataSetting.Speed;
        _dashSpeed = _playerDataSetting.DashSpeed;
        _dashDuration = _playerDataSetting.DashDuration;
        _dashEffectCount = _playerDataSetting.DashEffectCount;
        _playerDashFadeTime = _playerDataSetting.PlayerDashFadeTime;
        _dashCoolTime = _playerDataSetting.DashCoolTime;
        _invincibleTime = _playerDataSetting.InvincibleTime;
        MoveToStartPosition(new Vector3(-4,0,-4));
    }
    public void OnDetectedByChief()
    {
        IsInvincible = true;
        StartCoroutine(Invincible());
        itemLoseEffect.LoseEffectStart();
        StartCoroutine(ItemLostTextFadeOut());
        aim.SetTrigger("Hit");
        _isHitted = true;
        StartCoroutine(HittedCoroutine());
        SoundManager.Instance.PlayCommonSFXPitched("Hitted");
        Debug.Log("Player.OnDetectedByChief");
    }
    IEnumerator HittedCoroutine()
    {
        Rigidbody.velocity = Vector3.zero;
        yield return new WaitForSeconds(1.5f);
        _isHitted = false;
    }

    private IEnumerator Invincible()
    {
        StartCoroutine(InvincibleTimer());
        float alpha = -0.3f;
        while (IsInvincible)
        {
            var textColor = _playerSprite.color;
            textColor.a = 0.5f + alpha;
            _playerSprite.color = textColor;
            alpha = -alpha;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator InvincibleTimer()
    {
        yield return new WaitForSeconds(_invincibleTime);
        IsInvincible = false;
        var textColor = _playerSprite.color;
        textColor.a = 1f;
        _playerSprite.color = textColor;
    }
    public void OnStageChangeStart(bool initial = false)
    {
        aim.SetTrigger("Fly");
        itemLoseEffect.LoseEffectStart();
        if (!initial)
            StartCoroutine(ItemLostTextFadeOut());
        IsControlledByOther = true;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
    }
    public void OnStageChangeOver()
    {
        aim.SetTrigger("Idle");
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.constraints |= RigidbodyConstraints.FreezePositionY;
    }
    private void MoveToStartPosition(Vector3 pos)
    {
        //transform.position = pos;
        StartCoroutine(WaitUntilGameStart());
    }

    IEnumerator WaitUntilGameStart()
    {
        Debug.Log("Playre - Wait Until Game Start");
        //yield return new WaitUntil(() => GameSceneManager.Instance.IsOpeningAnimationFinished==true);
        isMoving = true;
        IsControlledByOther = false;
        Debug.Log("Player - Start Moving");
        StartCoroutine(StartMoving());
        yield break;
    }

    IEnumerator StartMoving()
    {
        _dashChargeEffect.gameObject.SetActive(true);
        while (isMoving)
        {
            if (GameManager.Instance.IsGamePause)
            {
                Debug.Log("Game Pause");
                yield return new WaitUntil(() => GameManager.Instance.IsGamePause == false);
                Debug.Log("Game Resume");
            }

            float root5unitMove = 0.56f;

            if (!IsControlledByOther && !_isHitted)
            {
                Vector3 moveDir = Vector3.zero;
                isWalking = false;
                if (Input.GetKey(KeyCode.LeftArrow))
                {
	                isWalking = true;
                    isFlipX = false;
                    SetPlayerFlipX();
                    moveDir += new Vector3(-root5unitMove, 0, root5unitMove);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                	isWalking = true;
                    isFlipX = true;
                    SetPlayerFlipX();
                    moveDir += new Vector3(root5unitMove, 0, -root5unitMove);
                }
                if (Input.GetKey(KeyCode.UpArrow))
                {
                	isWalking = true;
                    moveDir += new Vector3(1, 0, 1);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                	isWalking = true;
                    moveDir += new Vector3(-1, 0, -1);
                }
                //dash
                if (Input.GetKey(KeyCode.Space))
                {
                    if (!isDashCoolDown && !isDashing)
                    {
                        isDashing = true;
                        isDashCoolDown = true;
                        aim.SetBool("Dash",true);
                        SoundManager.Instance.PlayCommonSFXPitched("Dash");
                        aim.SetTrigger("DashTrigger");
                        _rigidbody.velocity = _lastDir * _dashSpeed;
                        StartCoroutine(Dash(_lastDir));
                        StartCoroutine(DashTimer());
                    }
                }
                if (!isDashing)
                {
                    _lastDir = moveDir;
                    _rigidbody.velocity = moveDir * _speed;
                    aim.SetBool("Walk", isWalking);
                    aim.SetFloat("MoveSpeed", _speed);
                }
                else
                {
                    _rigidbody.velocity = _lastDir * _dashSpeed;
                }
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsControlledByOther)
            return;
        var fishChief = other.transform.GetComponentInParent<FishCheif>();
        if (fishChief)
        {
            fishChief.OnHitPlayer(this);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Dealer"))
        {
            collision.gameObject.GetComponent<Dealer>().Exchange();
            Debug.Log("Player - OnTriggerEnter");
        }
    }

    private IEnumerator Dash(Vector3 dir)
    {
        StartCoroutine(DashEffectInstantiate());
        yield return new WaitForSeconds(_dashDuration);
        aim.SetBool("Dash",false);
        isDashing = false;
    }

    private IEnumerator DashEffectInstantiate()
    {
        var currentGreen = 1f;
        for (int i = 0; i < _dashEffectCount; i++)
        {
            var ob = Instantiate(_playerDashObject, null, false);
            ob.transform.position = transform.position+new Vector3(0.165f, 0.439f, 0.147f);
            ob.gameObject.SetActive(true);
            ob.GetComponent<SpriteRenderer>().flipX = isFlipX;
            currentGreen = Mathf.Lerp(1f, 0f, i / _dashEffectCount);
            StartCoroutine(DashEffect(ob, currentGreen));
            yield return new WaitForSeconds(_dashDuration / _dashEffectCount);
        }
    }

    private IEnumerator DashEffect(GameObject ob, float currentGreen)
    {
        var currentAlpha = 1f;
        for (float t = 0; t < _playerDashFadeTime; t += Time.deltaTime)
        {
            currentAlpha = Mathf.Lerp(1f, 0f, t / _playerDashFadeTime);
            var textColor = ob.GetComponent<SpriteRenderer>().color;
            textColor.a = currentAlpha;
            textColor.g= currentGreen;
            ob.GetComponent<SpriteRenderer>().color= textColor;
            yield return null;
        }
        ob.gameObject.SetActive(false);
        Destroy(ob);
    }

    private IEnumerator DashTimer()
    {
        _dashChargeEffect.gameObject.SetActive(false);
        yield return new WaitForSeconds(_dashCoolTime);
        isDashCoolDown = false;
        _dashChargeEffect.gameObject.SetActive(true);
    }
    
    
    
    public float fadeOutTime = 1.5f; // 페이드 아웃에 걸리는 시간 (초)
    private IEnumerator ItemLostTextFadeOut()
    {
        itemLoseImage.transform.position = transform.position+new Vector3(0, 1f, 0);
        itemLoseImage.transform.SetParent(null);
        itemLoseImage.gameObject.SetActive(true);
        var currentAlpha = 1f;
        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            currentAlpha = Mathf.Lerp(1f, 0f, t / fadeOutTime);
            SetTextAlpha(currentAlpha);
            itemLoseImage.transform.Translate(0, 0.5f * Time.deltaTime,0);
            yield return null;
        }
        currentAlpha = 0f;
        SetTextAlpha(currentAlpha);
        itemLoseImage.gameObject.SetActive(false);
    }

    private void SetTextAlpha(float alpha)
    {
        var textColor = itemLoseImage.color;
        textColor.a = alpha;
        itemLoseImage.color = textColor;
    }
}
