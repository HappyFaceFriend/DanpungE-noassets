using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishEndingAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image image9000;
    [SerializeField] private GameObject gogo;
    [SerializeField] private Image creditImage;
    [SerializeField] private Image creditOverLay;
    [SerializeField] private Image creditText;
    void Start()
    {
        SoundManager.Instance.PlayCommonBGM("FishBGM");
        creditImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [SerializeField] private float speed = 0.5f;
    public float fadeOutTime = 2f; // 페이드 아웃에 걸리는 시간 (초)

    public void OnCoinPaied()
    {

        StartCoroutine(FadeOut());
    }

    public void CreditStart()
    {
        StartCoroutine(KillFadeOut());
    }
    
    private IEnumerator FadeOut()
    {
        image9000.gameObject.SetActive(true);
        var currentAlpha = 1f;
        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            currentAlpha = Mathf.Lerp(1f, 0f, t / fadeOutTime);
            SetTextAlpha(currentAlpha);
            image9000.transform.Translate(0, speed * Time.deltaTime,0);
            yield return null;
        }
        currentAlpha = 0f;
        SetTextAlpha(currentAlpha);
        image9000.gameObject.SetActive(false);
    }

    private void SetTextAlpha(float alpha)
    {
        var textColor = image9000.color;
        textColor.a = alpha;
        image9000.color = textColor;
    }
    
    
    private IEnumerator KillFadeOut()
    {
        gogo.gameObject.SetActive(true);
        creditImage.gameObject.SetActive(true);
        creditOverLay.gameObject.SetActive(true);
        creditText.gameObject.SetActive(true);
        var currentAlpha = 0f;
        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            currentAlpha = Mathf.Lerp(0f, 1f, t / fadeOutTime);
            KillSetTextAlpha(currentAlpha);
            yield return null;
        }
        currentAlpha = 1f;
        KillSetTextAlpha(currentAlpha);
    }

    private void KillSetTextAlpha(float alpha)
    {
        var textColor = creditImage.color;
        var textColor1 = creditOverLay.color;
        var textColor2 = creditText.color;
        textColor.a = alpha;
        textColor1.a = alpha;
        textColor2.a = alpha;
        creditImage.color = textColor;
        creditOverLay.color = textColor1;
        creditText.color = textColor2;
    }
}
