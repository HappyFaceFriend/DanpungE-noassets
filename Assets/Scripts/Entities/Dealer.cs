using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI obtainText;
    [SerializeField] private float speed;
    [SerializeField] public float fadeOutTime; // 페이드 아웃에 걸리는 시간 (초)
    private bool isCollided = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Exchange()
    {
        int quotient = 0;
        int item = GameSceneManager.Instance.GetHoldingItemAmount();
        int MinAmount = GameSceneManager.Instance.MinItemSellAmount;
        if (item >= MinAmount)
        {
            quotient = item / MinAmount;
            GameSceneManager.Instance.SetHoldingItemAmount( item % MinAmount);
        }
        else
        {
            StartNotEnoughAnimation();
            return;
        }
        int getgold = quotient * GameSceneManager.Instance.ItemSellPrice * 10;
        GameManager.Instance.AddGold(getgold);
        //animation trigger
        StartObtainAnimation(getgold);
        GameSceneManager.Instance.UpdateText();
    }
    void Awake()
    {
        obtainText.gameObject.SetActive(false);
    }
    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
    }
    private void StartObtainAnimation(int gold)
    {
        obtainText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 1f, 0);
        obtainText.text = $"+{gold}";
        obtainText.gameObject.SetActive(true);
        StartCoroutine(FadeOut());
    }

    private void StartNotEnoughAnimation()
    {
        obtainText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 1f, 0);
        obtainText.text = $"Not Enough!";
        obtainText.gameObject.SetActive(true);
        StopAllCoroutines();
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
    }

    private void SetTextAlpha(float alpha)
    {
        var textColor = obtainText.color;
        textColor.a = alpha;
        obtainText.color = textColor;
    }
}
