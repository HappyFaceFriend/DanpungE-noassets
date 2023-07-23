using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform FollowTarget { get; set; }
    [SerializeField] float _smoothTime;
    void FixedUpdate()
    {
        if(FollowTarget != null)
        {
            Vector3 targetPos = new Vector3(FollowTarget.position.x, 0, FollowTarget.position.z);
            Vector3 velocity = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, _smoothTime);
        }
    }
}
