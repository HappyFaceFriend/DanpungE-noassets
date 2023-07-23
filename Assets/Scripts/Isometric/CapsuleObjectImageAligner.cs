using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class CapsuleObjectImageAligner: MonoBehaviour
{
    [SerializeField] CapsuleCollider _capsuleCollider;
    [SerializeField] bool _alignY = true;


    private void Awake()
    {
        //x = - , -2
    }

    public void Align()
    {
        if (_capsuleCollider == null)
        {
            _capsuleCollider = GetComponent<CapsuleCollider>();
            if (_capsuleCollider == null)
            {
                Debug.LogError("No Box Collider");
                return;
            }
        }
        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer spriteRenderer in spriteRenderers)
        {
            //Vector3 newPosition = -_boxCollider.size / 2;
            Vector3 newPosition = new Vector3(-_capsuleCollider.radius, -_capsuleCollider.height / 2,- _capsuleCollider.radius);
            if (!_alignY)
                newPosition.y = spriteRenderer.transform.localPosition.y;

            spriteRenderer.transform.localPosition = newPosition;

            spriteRenderer.transform.rotation = Quaternion.Euler(45, 45, spriteRenderer.transform.rotation.z);
        }
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(CapsuleObjectImageAligner))]
public class CapsuleObjectImageAlignerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CapsuleObjectImageAligner ob = (CapsuleObjectImageAligner)target;
        if (GUILayout.Button(new GUIContent("Align")))
        {
            ob.Align();
        }
    }
}
#endif
