using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindSpawner : MonoBehaviour
{
    [SerializeField] List<RectTransform> _windPrefabs;

    [SerializeField] float _interval = 0.2f;

    float _eTime = 0f;
    private void Start()
    {
        SoundManager.Instance.PlayCommonBGM("TitleWind");
        SoundManager.Instance.PlayCommonBGM("TitleBGM2");
    }
    private void Update()
    {
        _eTime += Time.deltaTime;
        if (_eTime > _interval )
        {
            _eTime -= _interval;

            RectTransform wind = Instantiate(_windPrefabs[Random.Range(0, _windPrefabs.Count)], transform);
            wind.anchoredPosition += new Vector2(Random.Range(-40, 40), Random.Range(-Screen.height / 2, Screen.height / 2));
        }
    }
}
