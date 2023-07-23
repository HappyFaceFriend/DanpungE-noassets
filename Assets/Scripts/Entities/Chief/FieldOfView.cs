using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] float _viewRadius;
    [Range(0, 360)][SerializeField] float _viewAngle;

    [SerializeField] LayerMask _targetMask;
    [SerializeField] LayerMask _obstacleMask;

    [SerializeField] Light2D _light;

    float _additionalRotation = 0;

    private void OnValidate()
    {
        _light.pointLightOuterRadius = _viewRadius;
        _light.pointLightOuterRadius = _viewRadius;
        _light.pointLightOuterAngle = _viewAngle;
        _light.pointLightInnerAngle = _viewAngle;
    }
    public float ViewRadius { get { return _viewRadius; } set { _viewRadius = value; } }
    public void SetAdditionalRotation(float y)
    {
        _additionalRotation = y;
    }
    private void OnDrawGizmos()
    {
        Vector3 myPos = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawWireSphere(myPos, _viewRadius);

        float lookingAngle = transform.eulerAngles.y + _additionalRotation;
        Vector3 rightDir = AngleToDir(lookingAngle + _viewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(lookingAngle - _viewAngle * 0.5f);
        Vector3 lookDir = AngleToDir(lookingAngle);

        Debug.DrawRay(myPos, rightDir * _viewRadius, Color.blue);
        Debug.DrawRay(myPos, leftDir * _viewRadius, Color.blue);
        Debug.DrawRay(myPos, lookDir * _viewRadius, Color.cyan);
    }
    Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    public List<Transform> GetTransformsInView()
    {
        List<Transform> hitTargetList = new List<Transform>();
        Collider[] targets = Physics.OverlapSphere(transform.position, _viewRadius, _targetMask);

        foreach (Collider collider in targets)
        {
            Vector3 targetPos = collider.transform.position;
            Vector3 targetDir = (targetPos - transform.position).normalized;
            Vector3 forward = (Quaternion.Euler(0, _additionalRotation, 0) * transform.forward);
            float targetAngle = Mathf.Acos(Vector3.Dot(forward, targetDir)) * Mathf.Rad2Deg;
            if (targetAngle <= _viewAngle * 0.5f
                && !Physics.Raycast(transform.position, targetDir, _viewRadius, _obstacleMask))
            {
                hitTargetList.Add(collider.transform);
            }
        }
        return hitTargetList;
    }
}