using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class FishCheif : ChiefBehaviour
{
    [SerializeField] Animator _bodyAnimator;
    [SerializeField] Animator _handAnimator;
    [SerializeField] float _hitDuration;
    [SerializeField] float _hitCooldown;
    bool _isHitting = false;

    bool _detectedPlayer = false;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    float eTime = 0f;
    protected override void Update()
    {
        base.Update();
        if (_isChasing && !_isHitting)
        {
            eTime += Time.deltaTime;
            if (eTime > _hitCooldown && !_detectedPlayer)
            {
                Hit();
                eTime = 0f;
            }
        }
        _bodyAnimator.SetInteger("State", (int)_currentState);

    }
    void Hit()
    {
        StartCoroutine(HitCoroutine());
    }
    public void OnHitPlayer(Player player)
    {
        player.OnDetectedByChief();
        StartCoroutine(DetectCoroutine());
    }
    IEnumerator DetectCoroutine()
    {
        _isChasing = false;
        _detectedPlayer = true;
        _nav.isStopped = true;
        yield return new WaitForSeconds(_hitCoolTime);
        _detectedPlayer = false;
        _nav.isStopped = false;
        _isChasing = true;
    }
    IEnumerator HitCoroutine()
    {
        _handAnimator.SetTrigger("Hit");
        _isHitting = true;
        yield return new WaitForSeconds(_hitDuration);
        _isHitting = false;
    }
    protected override void FlipImage()
    {
        if(_isHitting)
        {
            return;
        }
        var moveDir = transform.forward;
        if (moveDir.x > moveDir.z)
        {
            _image.localScale = new Vector3(-1, 1, 1);

        }
        else
        {
            _image.localScale = new Vector3(1, 1, 1);
        }
    }
}
