using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Vector3 dir;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            GetComponent<Rigidbody>().velocity += dir;
    }
}
