using System;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PopupOption:MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectSlider;

    public void Init()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        musicSlider.value = Bootstrapper.Instance.MusicVolume;
        effectSlider.value = Bootstrapper.Instance.EffectVolume;
    }
    
    public void SetMusicVolume()
    {
        Bootstrapper.Instance.MusicVolume = (int)musicSlider.value;
    }
    public void SetEffectVolume()
    {
        Bootstrapper.Instance.EffectVolume = (int)effectSlider.value;
    }
}