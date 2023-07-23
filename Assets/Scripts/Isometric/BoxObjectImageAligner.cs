using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class BoxObjectImageAligner : MonoBehaviour
{
    [SerializeField] BoxCollider _boxCollider;
    [SerializeField] bool _alignY = true;
    [SerializeField] bool _autoAlign = false;


    private void Awake()
    {
        //x = - , -2
    }
    public void OnDrawGizmosSelected()
    {
        if(_autoAlign)
            Align();
    }

    public void Align()
    {
        if (_boxCollider == null)
        {
            _boxCollider = GetComponent<BoxCollider>();
            if (_boxCollider == null)
            {
                Debug.LogError("No Box Collider");
                return;
            }
        }
        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer spriteRenderer in spriteRenderers)
        {
            Vector3 newPosition = -_boxCollider.size / 2;

            float ppu = spriteRenderer.sprite.pixelsPerUnit;
            newPosition += new Vector3(spriteRenderer.sprite.rect.width, 0, spriteRenderer.sprite.rect.height) / ppu / 2;

            if (!_alignY)
                newPosition.y = spriteRenderer.transform.localPosition.y;

            spriteRenderer.transform.localPosition = newPosition;

            spriteRenderer.transform.localRotation = Quaternion.Euler(26.57f, 45, spriteRenderer.transform.localRotation.eulerAngles.z);
        }
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(BoxObjectImageAligner))]
public class BoxObjectImageAlignerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BoxObjectImageAligner ob = (BoxObjectImageAligner)target;
        if (GUILayout.Button(new GUIContent("Align")))
        {
            ob.Align();
        }
    }
}
#endif
