using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] meleePrefabs;
    public GameObject[] rangePrefabs;
    private List<GameObject>[] _pools;
    private List<Enemy>[] _enemyPools;
    private List<Bullet>[] _meleePools;
    private List<Bullet>[] _rangePools;

    private void Awake()
    {
        _pools = new List<GameObject>[prefabs.Length];
        for (int i = 0; i < _pools.Length; i++)
            _pools[i] = new List<GameObject>();
        
        _enemyPools = new List<Enemy>[enemyPrefabs.Length];
        for (int i = 0; i < _enemyPools.Length; i++)
            _enemyPools[i] = new List<Enemy>();
        
        _meleePools = new List<Bullet>[meleePrefabs.Length];
        for (int i = 0; i < _meleePools.Length; i++)
            _meleePools[i] = new List<Bullet>();
        
        _rangePools = new List<Bullet>[rangePrefabs.Length];
        for (int i = 0; i < _rangePools.Length; i++)
            _rangePools[i] = new List<Bullet>();
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach (GameObject item in _pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            _pools[index].Add(select);
        }
        return select;
    }  
    public Enemy GetEnemy(int index)
    {
        Enemy select = null;

        foreach (Enemy item in _enemyPools[index])
        {
            if (!item.gameObject.activeSelf)
            {
                select = item;
                select.gameObject.SetActive(true);
                break;
            }
        }
        if (!select)
        {
            select = Instantiate(enemyPrefabs[index], transform).GetComponent<Enemy>();
            _enemyPools[index].Add(select);
        }
        return select;
    }
}
