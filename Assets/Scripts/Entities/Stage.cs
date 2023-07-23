using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public float Radius { get { return _radius; } }
    public float Size { get { return _size; } }
    [SerializeField] float _radius = 7f;
    [SerializeField] float _size = 5;


    [SerializeField] int _circularWallCount = 16;
    [SerializeField] BoxCollider _wallPrefab;
    [SerializeField] ChiefBehaviour _chief;
    public ItemSpawner ItemSpawner { get; private set; }

    [SerializeField] List<GameObject> _walls;
    private void Awake()
    {
        ItemSpawner = GetComponent<ItemSpawner>();
    }
    public void RemoveWalls()
    {
        foreach (GameObject wall in _walls)
        {
            wall.SetActive(false);
        }
        ItemSpawner.StopSpawning();
    }
    public void StopChasing()
    {
        _chief.StopChasing();
    }
    public void StartChasing()
    {
        _chief.StartChasing();
    }
    public void ResetChiefPosition()
    {
        _chief.ResetPosition();
    }
    public void SpawnWalls()
    {
        foreach (GameObject wall in _walls)
        {
            wall.SetActive(true);
        }
    }
    private void OnDrawGizmos()
    {
        var origin = transform.position;
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.color = new Color(0.8f, 0.5f, 0.5f);
        Gizmos.matrix = Matrix4x4.TRS(origin, Quaternion.identity, new Vector3(1, 0.02f, 1));
        Gizmos.DrawWireSphere(Vector3.zero, _radius);

        //Gizmos.matrix = Matrix4x4.TRS(origin, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), new Vector3(_size, 0, _size));
        //Gizmos.DrawWireCube(origin, Vector3.one);

        
        Gizmos.matrix = oldMatrix;
        Gizmos.DrawWireCube(origin, new Vector3(_size, 0, _size));
    }


    BoxCollider InstantiateWall(Vector3 position, float yRotation, Vector3 size)
    {
        BoxCollider wall = Instantiate(_wallPrefab, position, Quaternion.Euler(0, yRotation, 0));
        //wall.transform.SetParent(transform, false);
        wall.transform.localScale = size;
        wall.transform.SetParent(transform);
        _walls.Add(wall.gameObject);
        return wall;
    }
    public void CreateWalls()
    {
        _walls = new List<GameObject>();
        Vector3 myScale = new Vector3(_size, 0,  _size);
        Vector3 wallScale = _wallPrefab.size;

        float x = myScale.x / 2 + wallScale.x / 2;
        float z = myScale.z / 2 + wallScale.z / 2;
        //사각형
        InstantiateWall(transform.position + new Vector3(x, 0, 0), 0, new Vector3(_wallPrefab.size.x, _wallPrefab.size.y, _size));
        InstantiateWall(transform.position + new Vector3(-x, 0, 0), 0, new Vector3(_wallPrefab.size.x, _wallPrefab.size.y, _size));
        InstantiateWall(transform.position + new Vector3(0, 0, z), 0, new Vector3(_size, _wallPrefab.size.y, _wallPrefab.size.z));
        InstantiateWall(transform.position + new Vector3(0, 0, -z), 0, new Vector3(_size, _wallPrefab.size.y, _wallPrefab.size.z));

        //원
        float distance = _radius + wallScale.x / 2;
        for(int i=0; i<_circularWallCount; i++)
        {
            float angle = i * (Mathf.PI * 2 / _circularWallCount);

            Vector3 position = transform.position + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * distance;
            InstantiateWall(position, angle * Mathf.Rad2Deg + 90, _wallPrefab.size + new Vector3(0, 0, _size / 2));
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Stage))]
public class StageInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Stage ob = (Stage)target;
        if (GUILayout.Button(new GUIContent("Create Walls")))
        {
            ob.CreateWalls();
        }
    }
}
#endif
