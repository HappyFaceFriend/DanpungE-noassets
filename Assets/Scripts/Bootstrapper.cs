using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrapper : HappyTools.GameBootstrapper
{
    
    public override void InitGame()
    {
        
        MusicVolume = 100;
        EffectVolume = 100;
        Debug.Log("Bootstrapper - Init Game");
    }

}
