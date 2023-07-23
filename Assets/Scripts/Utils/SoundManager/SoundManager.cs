using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : HappyTools.SingletonBehaviour<SoundManager>
{
    Dictionary<int, AudioSource> _pitchedAudioSources = new Dictionary<int, AudioSource>();

    [SerializeField] GameObject _soundListParent;

    [SerializeField] AudioSource _sfxPlayer;
    [SerializeField] AudioSource _bgmPlayer;
    [SerializeField] AudioSource _bgmPlayer2;

    const int PitchPrecision = 1000;

    SoundList [] _soundLists;
    Dictionary<string, int> _soundListIndicies  = new Dictionary<string, int>();
    protected override void Awake()
    {
        base.Awake();

        _soundLists = _soundListParent.GetComponentsInChildren<SoundList>();
        for(int i = 0; i< _soundLists.Length; i++)
        {
            for (int j = 0; j < _soundLists[i].Datas.Count; j++)
                _soundListIndicies[_soundLists[i].Datas[j].Key] = i;
        }
        _pitchedAudioSources[1 * PitchPrecision] = _sfxPlayer;
    }
    public void PlaySFXPitched(AudioClip clip, float pitchMultiplier = 1f, float volumeMultiplier = 1f)
    {
        if (pitchMultiplier < 0)
            pitchMultiplier = 0.001f;
        int pitch = Mathf.RoundToInt(pitchMultiplier * PitchPrecision);
        if (!_pitchedAudioSources.ContainsKey(pitch))
        {
            _pitchedAudioSources[pitch] = _sfxPlayer.AddComponent<AudioSource>();
            _pitchedAudioSources[pitch].pitch = pitch / 1000f;
        }
        _pitchedAudioSources[pitch].PlayOneShot(clip, volumeMultiplier);
    }
    public void PlayBGM(AudioClip clip, float volumeMultiplier = 1f)
    {
        _bgmPlayer.Stop();
        _bgmPlayer.clip = clip;
        _bgmPlayer.volume = volumeMultiplier;
        _bgmPlayer.Play();
    }
    public void PlayBGM2(AudioClip clip, float volumeMultiplier = 1f)
    {
        _bgmPlayer2.Stop();
        _bgmPlayer2.clip = clip;
        _bgmPlayer2.volume = volumeMultiplier;
        _bgmPlayer2.Play();
    }
    public void StopBGM2()
    {
        _bgmPlayer2.Stop();
    }

    public void PlayCommonSFXPitched(string key, float pitchMultiplier = 1f, float volumeMultiplier = 1f)
    {
        if (_soundListIndicies.ContainsKey(key))
        {
            _soundLists[_soundListIndicies[key]].PlaySFXPitched(key, pitchMultiplier, volumeMultiplier);
        }
        else
        {
            Debug.LogWarning("No SFX matching: " + key);
        }
    }
    public void PlayCommonBGM(string key, float volumeMultiplier = 1f)
    {
        if (_soundListIndicies.ContainsKey(key))
        {
            if (key == "TitleWind")
                _soundLists[_soundListIndicies[key]].PlayBGM2(key, volumeMultiplier);
            else
                _soundLists[_soundListIndicies[key]].PlayBGM(key, volumeMultiplier);
        }
        else
        {
            Debug.LogWarning("No BGM matching: " + key);
        }
    }

}
