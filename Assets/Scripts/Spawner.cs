using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnDatas;
    private GameManager _gameManager;

    private float timer;
    private int _level;

    private void Start()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        _gameManager = GameManager.Instance;
    }
    void Update()
    {
        if (!_gameManager.isLive)
            return;
        timer += Time.deltaTime;

        _level = Mathf.Clamp(Mathf.FloorToInt(_gameManager.gameTime / 10f), 0, spawnDatas.Length - 1);
        if (timer > spawnDatas[_level].spawnTime)
        {
            Spawn();
            timer = 0f;
        }
    }

    void Spawn()
    {
        GameObject enemy = _gameManager.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnDatas[_level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public int spriteType;
    public float spawnTime;
    public int health;
    public float moveSpeed;
}
