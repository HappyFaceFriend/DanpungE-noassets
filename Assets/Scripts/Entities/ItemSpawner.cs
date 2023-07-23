using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] Stage _stage;
    [SerializeField] Player _player;
    [SerializeField] int _initialSpawn = 20;
    [SerializeField] float _newSpawnInterval = 2;
    [SerializeField] int _maxItemCount = 30;
    [SerializeField] int _maxItemCountPerSquare = 5;
    [SerializeField] float _edgePadding = 0.3f;
    [SerializeField] float _playerRadius = 1f;
    [SerializeField] GameObject _itemPrefab;
    [SerializeField] GameObject itemListObject;

    //[SerializeField] List<GameObject> _items = new List<GameObject>();

    Coroutine _spawningCoroutine;
    //9분면

    public int SpawnedItemAmount => Squares[0] + Squares[1] + Squares[2] + Squares[3];
    public List<int> Squares = Enumerable.Repeat(0, 4).ToList();

    public void StartSpawning()
    {
        InitialSpawn();
        Debug.Log("ItemSpawner - Start");
        _spawningCoroutine = StartCoroutine(SpawnCoroutine());
    }
    public void ResetCollectables()
    {
        foreach(var item in itemListObject.GetComponentsInChildren<DropItem>())
        {
            if (item.Owner == this)
                Destroy(item.gameObject);
        }
        for (int i = 0; i < Squares.Count; i++)
        {
            Squares[i] = 0;
        }
    }
    public void StopSpawning()
    {
        if(_spawningCoroutine != null)
            StopCoroutine(_spawningCoroutine);
    }
    public void InitialSpawn()
    {
        
        for(int i=0; i < _initialSpawn; i++)
        {
            SpawnSingle();
        }
    }
    IEnumerator SpawnCoroutine()
    {
        while(true)
        {
            if (GameManager.Instance.IsGamePause)
            {
                yield return new WaitUntil(() => GameManager.Instance.IsGamePause == false);
            }
            yield return new WaitForSeconds(_newSpawnInterval);
            if (SpawnedItemAmount < _maxItemCount)
            {
                SpawnSingle();
            }

        }
       // StartCoroutine(SpawnCoroutine());
    }
    public void SpawnSingle()
    {
        //랜덤 사분면 생성
        int idx = Random.Range(0, 4);
        while(Squares[idx]>= _maxItemCountPerSquare)
        {
            idx = Random.Range(0, 4);
        }
        float size = _stage.Size;
        float squareSize = size / 2;
        float startX = - size / 2+ (int)(idx/2) * squareSize;
        float startZ = - size / 2 + (idx % 2) * squareSize;
        float endX = startX + squareSize;
        float endZ = startZ + squareSize;
        if (idx / 2 == 0)
        {
            startX+= _edgePadding;
        }
        else
        {
            endX -= _edgePadding;
        }

        if (idx % 2 ==  0)
        {
            startZ+= _edgePadding;
        }
        else
        {
            startZ-= _edgePadding;
        }
        Vector3 position;
        
        while(true)
        {
            //네모 안에 생성
            position = _stage.transform.position + 
                new Vector3(
                    Random.Range(startX ,endX), 
                    _itemPrefab.transform.position.y, 
                    Random.Range( startZ, endZ));
            
            //원 안에 생성
            Vector3 d = (position - _stage.transform.position);
            float r = _stage.Radius - _edgePadding;
            if (d.x * d.x + d.z * d.z < r * r)
            {
                //충돌 체크
                Collider [] hitColliders = Physics.OverlapBox (
                    position,
                    new Vector3(0.5f,0.5f,0.5f), 
                    Quaternion.identity);
                bool isCollide = false;
                foreach (var collider in hitColliders)
                {
                    if (collider.gameObject.CompareTag("Building"))
                    {
                        isCollide = true;
                        break;
                    }
                }
                if (!isCollide)
                {
                    //플레이어 체크
                    if (Vector3.Distance(position, _player.transform.position) > _playerRadius)
                    {
                        //ok
                        break;
                    }
                }
            }
            else
            {
                while(Squares[idx]>= _maxItemCountPerSquare)
                {
                    idx = Random.Range(0, 4);
                    startX = - size / 2+ (int)(idx/2) * squareSize;
                    startZ = - size / 2 + (idx % 2) * squareSize;
                    endX = startX + squareSize;
                    endZ = startZ + squareSize;
                }
            }
        }
        Squares[idx]++;
        position.y = _itemPrefab.transform.position.y;
        var ob = Instantiate(_itemPrefab, position, Quaternion.identity);
        ob.transform.SetParent(itemListObject.transform);
        ob.GetComponent<DropItem>().Init(DropItemType.Crop, this);
        ob.GetComponent<DropItem>().SetSquareIdx(idx);
    }
}
