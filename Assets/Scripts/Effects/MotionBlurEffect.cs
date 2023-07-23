using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionBlurEffect : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 100f;
    [SerializeField] float _duration = 5.0f;
    [SerializeField] CameraController _camera;

    [SerializeField] Stage _farmStage;
    [SerializeField] Stage _fishStage;

    public IEnumerator SceneChangeCoroutine(MapType nextType)
    {
        Stage _nextStage;
        _camera.FollowTarget = null;
        if (nextType == MapType.FishingVillage)
            _nextStage = _fishStage;
        else
            _nextStage = _farmStage;
        Vector3 originalPos = transform.position;
        float eTime = 0f;
        while (eTime < _duration)
        {
            eTime += Time.deltaTime;
            yield return null;
            float t = eTime / _duration;
            transform.position = Vector3.Lerp(originalPos, _nextStage.transform.position, Utils.Curves.EaseInOut(t));
        }
        yield return null;
        GameSceneManager.Instance.SetHoldingItemAmount(0);
        GameSceneManager.Instance.SetItemIcon(nextType);
        transform.position = _nextStage.transform.position;
        //_camera.FollowTarget = GameSceneManager.Instance.Player.transform;
    }

    private void Update()
    {
    }
}
