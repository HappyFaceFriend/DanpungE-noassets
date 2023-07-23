using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private PopupOption popupOption;
    [SerializeField] private Image _fadeBlack;
    Animator _animator;
    // Start is called before the first frame update
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    void Start()
    {
        popupOption.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGameStartButtonClickEvent()
    {
        _animator.SetTrigger("Exit");
        SoundManager.Instance.PlayCommonSFXPitched("GameStart");
        SoundManager.Instance.PlayCommonSFXPitched("Click");
    }

    public void AnimEvent_LoadGameScene()
    {
        StartCoroutine(FadeAndGo("GameScene"));
    }

    IEnumerator FadeAndGo(string sceneName)
    {
        float eTime = 0f;
        float duration = 0.5f;
        while (eTime < duration)
        {
            eTime += Time.deltaTime;
            yield return null;
            float a = eTime / duration;
            Color c = new Color(0, 0, 0, a);
            _fadeBlack.color = c;
        }
        _fadeBlack.color = new Color(0, 0, 0, 1);
        SceneManager.LoadScene(sceneName);
    }
    public void OnOptionButtonClickEvent()
    {
        popupOption.gameObject.SetActive(true);
        SoundManager.Instance.PlayCommonSFXPitched("Click");
    }
    public void OnOptionPopupCloseButtonClicked()
    {
        popupOption.gameObject.SetActive(false);
        SoundManager.Instance.PlayCommonSFXPitched("Click");
    }
}
