using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffectManager : MonoBehaviour
{
    [SerializeField] List<WindEffect> _windPrefabs;
    [SerializeField] float _speed = 30f;
    [SerializeField] float _randomizer = 1f;
    [SerializeField] float _interval = 0.1f;
    [SerializeField] float _minZ;
    [SerializeField] float _maxZ;
    [SerializeField] float _xOffset = 19;

    [SerializeField] GameObject _windFade;
    float eTime = 0f;
    private void OnEnable()
    {

        SoundManager.Instance.PlayCommonSFXPitched("Wind");
        _windFade.SetActive(true);
    }
    private void OnDisable()
    {
        _windFade.GetComponent<Animator>().SetTrigger("Kill");
    }
    public void Update()
    {
        eTime += Time.deltaTime;
        if(eTime > _interval)
        {
            eTime -= _interval;
            SpawnWind();
        }
    }
    void SpawnWind()
    {
        WindEffect wind = Instantiate(_windPrefabs[Random.Range(0, _windPrefabs.Count)]);
        float z = Random.Range(_minZ, _maxZ);
        //wind.transform.SetParent(transform);
        if (transform.localScale.x < 0)
        {
            wind.transform.position = - new Vector3(z + _xOffset , 0f, z) + transform.position;
            wind.transform.localScale = new Vector3(-1, 1, 1);
            wind.StartMove(-_speed * Random.Range((1 - _randomizer), (1 + _randomizer)));
        }
        else
        {
            
            wind.transform.position = new Vector3(z + _xOffset, 0, z) + transform.position;
            wind.StartMove(_speed * Random.Range((1 - _randomizer), (1 + _randomizer)));
        }
    }

}
