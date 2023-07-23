using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PopupPause:MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionButton;

    public void Init()
    {
        gameObject.SetActive(false);
    }
    public void OnResumeButtonClicked()
    {
        gameObject.SetActive(false);

        SoundManager.Instance.PlayCommonSFXPitched("Click");
        GameManager.Instance.ResumeGame();
    }
}