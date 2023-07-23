using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UI;
public class PlayerDollMovement : MonoBehaviour
{
    [SerializeField] float _verticalSpeed = 1;
    [SerializeField] float _verticalMoveAmount = 100;
    [SerializeField] float _movementSpeed = 100f;
    [SerializeField] float _horizontalMoveAmount = 30;
    [SerializeField] float _rotAmount = 30;
    [SerializeField] float _horizontalDuration = 0.5f;
    RectTransform _rectTransform;

    float _eTime = 0f;
    float originalY;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        originalY = _rectTransform.localPosition.y;
    }
    private void Start()
    {
        StartCoroutine(HMoveCoroutine(false));
    }
    private void Update()
    {
        _eTime += Time.deltaTime;

        Vector2 moveVec = new Vector3(0, originalY);

        moveVec.y += Mathf.Sin(_verticalSpeed * _eTime) * _verticalMoveAmount;
        
        _rectTransform.anchoredPosition = moveVec;
    }
    IEnumerator HMoveCoroutine(bool toggle)
    {
        float dTime = 0f;
        float duration = _horizontalDuration * Random.Range(0.8f, 1.2f);

        float x = Random.Range(_horizontalMoveAmount / 2, _horizontalMoveAmount);
        if (toggle)
            x *= -1;
        float targetRot = 0f;
        if (x > _rectTransform.anchoredPosition.x)
        {
            targetRot = -Random.Range(_rotAmount * 0.5f, _rotAmount);
        }
        else
        {
            targetRot = Random.Range(_rotAmount * 0.5f, _rotAmount);
        }
        Quaternion originalRot = _rectTransform.localRotation;
        float originalX = _rectTransform.anchoredPosition.x;
        while(dTime < duration)
        {
            dTime += Time.deltaTime;
            yield return null;
            float t = Utils.Curves.EaseOut(dTime / duration);
            _rectTransform.localRotation = Quaternion.Lerp(originalRot, Quaternion.Euler(0, 0, targetRot), t);
            _rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(originalX,x, t), _rectTransform.anchoredPosition.y);
        }
        yield return StartCoroutine(HMoveCoroutine(!toggle));

    }
}
