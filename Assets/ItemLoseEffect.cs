using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLoseEffect : MonoBehaviour
{
    public ParticleSystem _particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoseEffectStart()
    {
        _particleSystem.emission.SetBurst(0,new ParticleSystem.Burst(0,GameSceneManager.Instance.GetHoldingItemAmount()));
       GameSceneManager.Instance.SetHoldingItemAmount(0);
       GameSceneManager.Instance.UpdateText();
        _particleSystem.Play();
    }
}
