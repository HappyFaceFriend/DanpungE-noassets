using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ChiefBehaviour : MonoBehaviour
{
    protected NavMeshAgent _nav;
    [SerializeField] protected Transform _image;

    protected bool _startPermission = false;
    protected bool _isChasing = false;
    protected Quaternion _originalImageRotation;

    protected enum State { Idle = 0, Walk = 1 };
    protected State _currentState;

    [SerializeField] protected Transform  _flashLightAnchor;
    Quaternion _flashAnchorTargetRot;
    Vector3 _flashAnchorTargetRotOriginal;
    [SerializeField] float _flashRotationSpeed;
    [SerializeField] float _startCoolTime = 3f;
    [SerializeField] protected float _hitCoolTime = 3f;

    Vector3 _startPosition;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        _nav = GetComponent<NavMeshAgent>();
        _originalImageRotation = _image.rotation;
        _flashAnchorTargetRot = _flashLightAnchor.rotation;
        _flashAnchorTargetRotOriginal = _flashLightAnchor.rotation.eulerAngles;
        _currentState = State.Idle;
        _startPosition = transform.position;
    }
    bool _initial = true;
    float _eTime = 0f;
    public void StartChasing()
    {
        _startPermission = true;
        _eTime = 0f;
        _initial = true;
        _nav.isStopped = false;
    }
    public void ResetPosition()
    {
        transform.position = _startPosition;
    }
    public void StopChasing()
    {
        _isChasing = false;
        _startPermission = false;
        _nav.isStopped = true;
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }

    private void OnWillRenderObject()
    {
        if (_initial && _isChasing == false && _startPermission == true)
        {
            _isChasing = true;
            _initial = false;
        }
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (_initial && _isChasing == false && _startPermission == true)
        {
            _eTime += Time.deltaTime;
            if (_eTime > _startCoolTime)
            {
                _isChasing = true;
                _initial = false;
            }
        }
        _flashLightAnchor.rotation = Quaternion.RotateTowards(_flashLightAnchor.rotation, _flashAnchorTargetRot, _flashRotationSpeed * Time.deltaTime);
        if (_isChasing)
        {
            if (_nav.remainingDistance > 0.1f)
                _currentState = State.Walk;
            else
                _currentState = State.Idle;

            _nav.destination = GameSceneManager.Instance.Player.transform.position;
            _image.rotation = _originalImageRotation;
            //var moveDir = (_nav.path.corners[1] - transform.position).normalized;
            FlipImage();
        }
        else
            _currentState = State.Idle;

    }
    protected virtual void FlipImage()
    {

        var moveDir = transform.forward;
        if (moveDir.x > moveDir.z)
        {
            _image.localScale = new Vector3(-1, 1, 1);
            _flashAnchorTargetRot = Quaternion.Euler(_flashAnchorTargetRotOriginal + new Vector3(0, 180, 0));

        }
        else
        {
            _flashAnchorTargetRot = Quaternion.Euler(_flashAnchorTargetRotOriginal);
            _image.localScale = new Vector3(1, 1, 1);
        }
    }
}
