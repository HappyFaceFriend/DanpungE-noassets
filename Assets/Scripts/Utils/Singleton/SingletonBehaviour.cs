using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HappyTools
{
    /// <summary>
    /// This SingletonBehaviour logs null if instance is not created and tries to access it.
    /// If another instance of this is created, the old instance is destroyed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));

                    if (instance == null)
                        Debug.LogError("No object of " + typeof(T).Name + " is no found");
                }
                return instance;
            }
        }
        static T instance = null;

        public virtual void Init()
        {
            
        }

        protected virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(Instance.gameObject);
            }

            instance = GetComponent<T>();
            Debug.Log(instance);
            if(transform.parent == null)
                DontDestroyOnLoad(gameObject);
        }
    }

}