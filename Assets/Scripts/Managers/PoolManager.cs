using System.Collections.Generic;
using Core;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public GameObject enemyPrefabs;
    public GameObject[] meleePrefabs;
    public GameObject[] rangePrefabs;
    public GameObject[] expPrefabs;
    public GameObject[] boxPrefabs;
    
    private List<Enemy> _enemyPools;
    private List<Bullet>[] _meleePools;
    private List<Bullet>[] _rangePools;
    private List<Exp>[] _expPools;
    private List<Box>[] _boxPools;

    protected override void AwakeInit()
    {
        _enemyPools = new List<Enemy>();
        
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

    public void DestroyAllObjects()
    {
        foreach (var enemy in _enemyPools)
            Destroy(enemy.gameObject);
        _enemyPools.RemoveRange(0, _enemyPools.Count);
        
        foreach (var meleePool in _meleePools)
        {
            foreach (var melee in meleePool)
                Destroy(melee.gameObject);
            meleePool.RemoveRange(0, meleePool.Count);
        }
        foreach (var rangePool in _rangePools)
        {
            foreach (var range in rangePool)
                Destroy(range.gameObject);
            rangePool.RemoveRange(0, rangePool.Count);
        }
        foreach (var expPool in _expPools)
        {
            foreach (var exp in expPool)
                Destroy(exp.gameObject);
            expPool.RemoveRange(0, expPool.Count);
        }
        foreach (var boxPool in _boxPools)
        {
            foreach (var box in boxPool)
                Destroy(box.gameObject);
            boxPool.RemoveRange(0, boxPool.Count);
        }
    }

    public void DisableAllObjects()
    {
        foreach (Enemy enemy in _enemyPools)
        {
            enemy.Die(false);
        }
        foreach (List<Bullet> meleePool in _meleePools)
        {
            foreach (var melee in meleePool)
                melee.gameObject.SetActive(false);
        }
        foreach (List<Bullet> rangePool in _rangePools)
        {
            foreach (var range in rangePool)
                range.gameObject.SetActive(false);
        }
        foreach (List<Exp> expPool in _expPools)
        {
            foreach (var exp in expPool)
                exp.gameObject.SetActive(false);
        }
        foreach (List<Box> boxPool in _boxPools)
        {
            foreach (Box box in boxPool)
                box.gameObject.SetActive(false);
        }
    }

    public Enemy GetEnemy()
    {
        Enemy select = null;

        foreach (Enemy item in _enemyPools)
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
            select = Instantiate(enemyPrefabs, transform).GetComponent<Enemy>();
            _enemyPools.Add(select);
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
