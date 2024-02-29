using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] meleePrefabs;
    public GameObject[] rangePrefabs;
    public GameObject[] expPrefabs;
    public GameObject[] boxPrefabs;
    
    private List<Enemy>[] _enemyPools;
    private List<Bullet>[] _meleePools;
    private List<Bullet>[] _rangePools;
    private List<Exp>[] _expPools;
    private List<Box>[] _boxPools;

    private void Awake()
    {
        _enemyPools = new List<Enemy>[enemyPrefabs.Length];
        for (int i = 0; i < _enemyPools.Length; i++)
            _enemyPools[i] = new List<Enemy>();
        
        _meleePools = new List<Bullet>[meleePrefabs.Length];
        for (int i = 0; i < _meleePools.Length; i++)
            _meleePools[i] = new List<Bullet>();
        
        _rangePools = new List<Bullet>[rangePrefabs.Length];
        for (int i = 0; i < _rangePools.Length; i++)
            _rangePools[i] = new List<Bullet>();
        
        _expPools = new List<Exp>[expPrefabs.Length];
        for (int i = 0; i < _expPools.Length; i++)
            _expPools[i] = new List<Exp>();
        
        _boxPools = new List<Box>[boxPrefabs.Length];
        for (int i = 0; i < _boxPools.Length; i++)
            _boxPools[i] = new List<Box>();
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
       
    public Bullet GetMelee(int index)
    {
        Bullet select = null;

        foreach (Bullet item in _meleePools[index])
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
            select = Instantiate(meleePrefabs[index], transform).GetComponent<Bullet>();
            _meleePools[index].Add(select);
        }
        return select;
    }   
    public Bullet GetRange(int index)
    {
        Bullet select = null;

        foreach (Bullet item in _rangePools[index])
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
            select = Instantiate(rangePrefabs[index], transform).GetComponent<Bullet>();
            _rangePools[index].Add(select);
        }
        return select;
    }
    
    public Exp GetExp(int index)
    {
        Exp select = null;

        foreach (Exp item in _expPools[index])
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
            select = Instantiate(expPrefabs[index], transform).GetComponent<Exp>();
            _expPools[index].Add(select);
        }
        return select;
    }    
    public Box GetBox(int index)
    {
        Box select = null;

        foreach (Box item in _boxPools[index])
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
            select = Instantiate(boxPrefabs[index], transform).GetComponent<Box>();
            _boxPools[index].Add(select);
        }
        return select;
    }

    public List<Exp> GetAllExp(int index)
    {
        return _expPools[index];
    }
}
