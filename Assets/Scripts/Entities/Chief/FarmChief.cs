using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class FarmChief : ChiefBehaviour
{
    [SerializeField] FieldOfView _fov;

    Animator _animator;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        _animator.SetFloat("WalkSpeed", _nav.speed);
        if (_isChasing)
        {
            _flashLightAnchor.gameObject.SetActive(true);
        }
        else
        {
            _flashLightAnchor.gameObject.SetActive(false);
        }
        if (_isChasing)
        {
            if (_fov != null)
            {
                var detected = _fov.GetTransformsInView();
                foreach (Transform t in detected)
                {
                    Player player = t.GetComponent<Player>();
                    if (player != null && !player.IsInvincible)
                    {
                        player.OnDetectedByChief();
                        StartCoroutine(DetectCoroutine());
                    }
                }
            }

        }

        _animator.SetInteger("State", (int)_currentState);

    }
    IEnumerator DetectCoroutine()
    {
        _isChasing = false;
        _nav.isStopped = true;
        yield return new WaitForSeconds(_hitCoolTime);
        _isChasing = true;
        _nav.isStopped = false;
    }
}
