using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EndingScene:MonoBehaviour
{
    [SerializeField] private Image sceneChangeCurtain ;
    [SerializeField] public float fadeOutTime; // 페이드 아웃에 걸리는 시간 (초)
    public void  Init()
    {
        StartEndingSceneInitCurtainAnimation();
    }
    //시작할때 검정 화면 페이드 아웃
    private void StartEndingSceneInitCurtainAnimation()
    {
        //StopAllCoroutines();
        Debug.Log("CurtainFadeOutStart");
        StartCoroutine(CurtainFadeOut());
    }
    private IEnumerator CurtainFadeOut()
    {
        
        sceneChangeCurtain.gameObject.SetActive(true);
        var currentAlpha = 1f;
        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            currentAlpha = Mathf.Lerp(1f, 0f, t / fadeOutTime);
            SetTextAlpha(currentAlpha);
            yield return null;
        }
        currentAlpha = 0f;
        SetTextAlpha(currentAlpha);
        sceneChangeCurtain.gameObject.SetActive(false);
        Debug.Log("CurtainFadeOutEnd");
    }

    private void SetTextAlpha(float alpha)
    {
        var textColor = sceneChangeCurtain.color;
        textColor.a = alpha;
        sceneChangeCurtain.color = textColor;
    }

    
}