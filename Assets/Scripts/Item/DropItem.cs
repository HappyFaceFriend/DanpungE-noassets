using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer obtainText;
    [SerializeField] private SpriteRenderer itemImage;
    private DropItemType _dropItemType;

    [SerializeField] private float speed = 0.5f;
    public float fadeOutTime = 2f; // 페이드 아웃에 걸리는 시간 (초)
    public int SquareIdx = -1;
    private bool isCollided = false;
    public UnityEvent onObtain;
    public ItemSpawner Owner;

    void Awake()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        isCollided = false;
        //GetComponentInChildren<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
    }
    void Start()
    {
        onObtain.AddListener(GameSceneManager.Instance.OnObtainItem);
    }
    public void Init(DropItemType type, ItemSpawner owner)
    {
        this._dropItemType = type;
        itemImage.gameObject.SetActive(true);
        obtainText.gameObject.SetActive(false);
        Owner = owner;
        StartCoroutine(ScalePulseOnStart());
    }

    private IEnumerator ScalePulseOnStart()
    {
        yield break;
    }

    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
        //initialized at itemSpawner
    }

    public void SetSquareIdx(int idx)
    {
        SquareIdx = idx;
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    public void OnTriggerStay(Collider other)
    {
        if (!isCollided)
        {
            isCollided = true;
            if (!other.gameObject.CompareTag("Player")) return;
            GameSceneManager.Instance.onObtainItem.Invoke();
            SoundManager.Instance.PlayCommonSFXPitched("Collect");
            StartObtainAnimation();
            Debug.Log("OnTriggerStay");
        }
    }


    private void StartObtainAnimation()
    {
        itemImage.gameObject.SetActive(false);
        obtainText.transform.position = transform.position+new Vector3(0, 1f, 0);
        obtainText.gameObject.SetActive(true);
        GameSceneManager.Instance.DecrementItem(SquareIdx);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        var currentAlpha = 1f;
        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            currentAlpha = Mathf.Lerp(1f, 0f, t / fadeOutTime);
            SetTextAlpha(currentAlpha);
            obtainText.transform.Translate(0, speed * Time.deltaTime,0);
            yield return null;
        }
        currentAlpha = 0f;
        SetTextAlpha(currentAlpha);
        obtainText.gameObject.SetActive(false);
        
        Destroy(gameObject);
    }

    private void SetTextAlpha(float alpha)
    {
        var textColor = obtainText.color;
        textColor.a = alpha;
        obtainText.color = textColor;
    }


}