using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnDatas;
    public float levelTime;
    private GameManager _gameManager;

    private float _timer;
    private int _level;

    private void Start()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        _gameManager = GameManager.Instance;
        levelTime = _gameManager.maxGameTime / spawnDatas.Length;
    }
    void Update()
    {
        if (!_gameManager.isLive)
            return;
        _timer += Time.deltaTime;

        _level = Mathf.Clamp(Mathf.FloorToInt(_gameManager.gameTime / levelTime), 0, spawnDatas.Length - 1);
        if (_timer > spawnDatas[_level].spawnTime)
        {
            Spawn();
            _timer = 0f;
        }
    }

    void Spawn()
    {
        Enemy enemy = _gameManager.pool.GetEnemy(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.Init(spawnDatas[_level]);
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
