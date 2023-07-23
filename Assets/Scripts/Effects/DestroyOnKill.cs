using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnKill : MonoBehaviour
{
    public void Kill()
    {
        Destroy(gameObject);
    }
}
