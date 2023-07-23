using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HappyTools
{
    public abstract class GameBootstrapper : SingletonBehaviourFixed<GameBootstrapper>
    {
        public int MusicVolume;
        public int EffectVolume;

        /// <summary>
        /// This is called right before the scene loads, before all Awake() calls.
        /// </summary>
        public virtual void InitGame()
        {
            
        }
    }
}
