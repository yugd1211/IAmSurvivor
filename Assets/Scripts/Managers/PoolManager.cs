using System.Collections.Generic;
using Core;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public partial class PoolManager
{
    public GameObject enemyPrefabs;
    public GameObject expPrefabs;
    public GameObject boxPrefabs;
    public GameObject[] meleePrefabs;
    public GameObject[] rangePrefabs;
    private void InitCreatePools()
    {
        CreatePool<Enemy>(enemyPrefabs);
        CreatePool<Exp>(expPrefabs);
        CreatePool<Box>(boxPrefabs);
        
        CreatePool<Bullet>(meleePrefabs[0], (int)Weapon.BulletType.Shovels);
        CreatePool<Bullet>(meleePrefabs[1], (int)Weapon.BulletType.Poke);
        CreatePool<Bullet>(meleePrefabs[2], (int)Weapon.BulletType.Scythe);
        CreatePool<Bullet>(rangePrefabs[0], (int)Weapon.BulletType.Sniper);
        CreatePool<Bullet>(rangePrefabs[1], (int)Weapon.BulletType.MachineGun);
        CreatePool<Bullet>(rangePrefabs[2], (int)Weapon.BulletType.Shotgun);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
            InitCreatePools();
    }
    
    protected override void AwakeInit()
    {
        InitCreatePools();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
}

public partial class PoolManager : Singleton<PoolManager>
{
    private readonly Dictionary<Type, Dictionary<int, object>> _poolDic = new Dictionary<Type, Dictionary<int, object>>();
    
    public void CreatePool<T>(GameObject prefab, int id = 0) where T : Component
    {
        if (_poolDic.ContainsKey(typeof(T)))
        {
            if (_poolDic[typeof(T)].ContainsKey(id))
                return;
        }
        else
        {
            _poolDic.Add(typeof(T), new Dictionary<int, object>());
        }

        ObjectPool<T> newPool = new ObjectPool<T>(prefab, 1, transform);
        _poolDic[typeof(T)].Add(id, newPool);
    }

    public T Get<T>(int index = 0) where T : Component
    {
        if (_poolDic.ContainsKey(typeof(T)) && _poolDic[typeof(T)].TryGetValue(index, out object pool))
            return ((ObjectPool<T>)pool)?.Get();
        else
            Debug.Log($"No pool type = {typeof(T)}, index = {index}.");
        return null;
    }
    
    public List<T> GetAll<T>(int index = 0) where T : Component
    {
        if (_poolDic.ContainsKey(typeof(T)) && _poolDic[typeof(T)].TryGetValue(index, out object pool))
            return ((ObjectPool<T>)pool).GetAll();
        else
            Debug.Log($"No pool type = {typeof(T)}, index = {index}.");
        return null;
    }
    
    public void ExecuteMethodOnAllObjects(string method)
    {
        foreach (object objectPool in _poolDic.Values.SelectMany(pools => pools.Values))
            objectPool.GetType().GetMethod(method)?.Invoke(objectPool, null);
    }
}

public class ObjectPool<T> where T : Component
{
    private readonly GameObject _prefab;
    private readonly Transform _parentTransform;
    private readonly List<T> _pool;
    private int _currPos;

    public ObjectPool(GameObject prefab, int initSize, Transform parentTransform)
    {
        _currPos = 0;
        _prefab = prefab;
        _parentTransform = parentTransform;
        _pool = new List<T>();

        for (int i = 0; i < initSize; i++)
            AddObject();
    }

    public T Get()
    {
        if (_currPos >= _pool.Count)
            _currPos = 0;
        for (; _currPos < _pool.Count; _currPos++)
        {
            if (_pool[_currPos].gameObject.activeSelf)
                continue;
            _pool[_currPos].gameObject.SetActive(true);
            return _pool[_currPos++];
        }
        AddObject();
        _pool[_currPos].gameObject.SetActive(true);
        return _pool[_currPos++];
    }

    public List<T> GetAll()
    {
        return _pool;
    }

    private void AddObject()
    {
        T newObject = Object.Instantiate(_prefab, _parentTransform)?.GetComponent<T>();
        _pool.Add(newObject);
        newObject?.gameObject.SetActive(false);
    }
    
    public void DisableAll()
    {
        foreach (T item in _pool)
            item.gameObject.SetActive(false);
    }
    public void DestroyAll()
    {
        foreach (T item in _pool)
            Object.Destroy(item.gameObject);
        _pool.Clear();
    }
}
