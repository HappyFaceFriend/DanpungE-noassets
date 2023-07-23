using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EndingSceneManager:HappyTools.SingletonBehaviour<EndingSceneManager>
{
    [SerializeField] private EndingScene endingScene;
    void Start()
    {
        endingScene.Init();
    }
}