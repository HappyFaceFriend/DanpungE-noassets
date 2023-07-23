using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InGameUISet:MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI holdingItemAmountText;
    [SerializeField] private TextMeshProUGUI WindTimeText;
    [SerializeField] private GameObject WindUIObject;
    [SerializeField] private Button pauseButton;
    [SerializeField] private PopupPause popupPause;
    [SerializeField] private PopupOption popupOption;
    [SerializeField] private PopupExitConfirm popupExit;
    [SerializeField] private Image windWarningFrame;
    [SerializeField] private Image itemIconFish;
    [SerializeField] private Image itemIconPersimmon;

    List<Coroutine> _coroutinesToStopOnForcedFly = new List<Coroutine>();
    public void Start()
    {
        popupPause.gameObject.SetActive(false);
        popupOption.gameObject.SetActive(false);
        popupExit.gameObject.SetActive(false);
        windWarningFrame.gameObject.SetActive(false);
        WindUIObject.gameObject.SetActive(false);
        itemIconFish.gameObject.SetActive(false);
        itemIconPersimmon.gameObject.SetActive(true);
        UpdateText();
    }
    public void OnObtainItem()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        int gold = GameManager.Instance.GetGold();
        goldText.text = $"{gold} / 9000";
        int item = GameSceneManager.Instance.GetHoldingItemAmount();
        holdingItemAmountText.text = $"{item}";
    }
    public void OnPauseButtonClicked()
    {
        popupPause.gameObject.SetActive(true);
        GameManager.Instance.PauseGame();

        SoundManager.Instance.PlayCommonSFXPitched("Click");
    }
    

    public void OnOptionButtonClicked()
    {
        popupPause.gameObject.SetActive(false);
        popupOption.gameObject.SetActive(true);

        SoundManager.Instance.PlayCommonSFXPitched("Click");
        //option menu
    }

    public void OnOptionPopupCloseButtonClicked()
    {
        popupOption.gameObject.SetActive(false);
        popupPause.gameObject.SetActive(true);

        SoundManager.Instance.PlayCommonSFXPitched("Click");
    }

    public void OnMainMenuButtonClicked()
    {
        popupPause.gameObject.SetActive(false);
        popupExit.gameObject.SetActive(true);

        SoundManager.Instance.PlayCommonSFXPitched("Click");
    }

    public void OnMainMenuButtonNoButtonClicked()
    {
        popupExit.gameObject.SetActive(false);
        popupPause.gameObject.SetActive(true);

        SoundManager.Instance.PlayCommonSFXPitched("Click");
    }
    public void OnMainMenuButtonYesButtonClicked()
    {
        SceneManager.LoadScene("MainMenuScene");
        SoundManager.Instance.PlayCommonSFXPitched("Click");
    }

    public void OnForceFly()
    {
        foreach (Coroutine c in _coroutinesToStopOnForcedFly)
            StopCoroutine(c);
        _coroutinesToStopOnForcedFly.Clear();
    }

    public void SetWindTimer()
    {
        _coroutinesToStopOnForcedFly.Add(StartCoroutine(FadeOutRoutine()));
    }
    
    public float fadeOutTime = 0.8f; // 페이드 아웃에 걸리는 시간 (초)
    private IEnumerator FadeOutRoutine()
    {
        SoundManager.Instance.PlayCommonSFXPitched("Warning");
        WindUIObject.SetActive(true);

        _coroutinesToStopOnForcedFly.Add(StartCoroutine(UpdateWindText()));

        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(5);
        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(FadeOut());
            if (i == 1)
                SoundManager.Instance.PlayCommonSFXPitched("Warning");
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(2);
        WindUIObject.SetActive(false);
    }

    private IEnumerator UpdateWindText()
    {
        int t = 10;
        while (t >= 0)
        {
            WindTimeText.text = $"00:{t:D2}";
            yield return new WaitForSeconds(1);
            t--;
        }
    }
    private IEnumerator FadeOut()
    {
        windWarningFrame.gameObject.SetActive(true);
        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            var currentAlpha = Mathf.Lerp(1f, 0f, t / fadeOutTime);
            var textColor = windWarningFrame.color;
            textColor.a = currentAlpha;
            windWarningFrame.color = textColor;
            yield return null;
        }
        windWarningFrame.gameObject.SetActive(false);
    }

    public void SetItemIcon(bool isFarm)
    {
        itemIconFish.gameObject.SetActive(!isFarm);
        itemIconPersimmon.gameObject.SetActive(isFarm);
    }
}