using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnKill : MonoBehaviour
{
    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
