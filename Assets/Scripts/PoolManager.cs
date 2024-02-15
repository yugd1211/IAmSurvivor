using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    public int initNum = 500;
    private List<GameObject>[] _pools;

    private void Awake()
    {
        _pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < _pools.Length; i++)
            _pools[i] = new List<GameObject>();
        for (int i = 0; i < _pools.Length; i++)
        {
            for (int j = 0; j < initNum; j++)
            {
                _pools[i].Add(Instantiate(prefabs[i], transform));
                _pools[i][j].SetActive(false);
            }
        }
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
}
