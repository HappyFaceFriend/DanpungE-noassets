using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] Vector3 _windAccel;

    private void OnTriggerStay(Collider other)
    {
        var player = other.GetComponent<Player>();

        if ( player != null)
        {
            other.attachedRigidbody.AddForce(_windAccel);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + _windAccel);
    }
}
