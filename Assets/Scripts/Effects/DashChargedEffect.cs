using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DashChargedEffect : MonoBehaviour
{
    [SerializeField] Light2D _light;

    [SerializeField] float _speed;
    [SerializeField] float _minIntensity;
    [SerializeField] float _maxIntensity;

    float _eTime = 0f;

    private void Update()
    {
        _eTime += Time.deltaTime * _speed;
        _light.intensity = _minIntensity + (Mathf.Sin(_eTime) + 1f) / 2f * (_maxIntensity - _minIntensity);
    }
}
