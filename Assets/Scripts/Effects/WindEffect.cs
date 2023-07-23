using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour
{
    public float _moveSpeed;
    public void StartMove(float speed)
    {
        _moveSpeed = speed;
    }

    private void Update()
    {
        transform.position += new Vector3(-_moveSpeed, 0, _moveSpeed) * Time.deltaTime;
    }
    public void Kill()
    {
        Destroy(gameObject);
    }
}
