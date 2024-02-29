using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public EnemyData[] enemyDatas;
    public EnemyData bossData;
    public float[] spawnTime;
    public float levelTime;
    
    private GameManager _gameManager;

    private float _timer;
    private int _level;

    private void Start()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        _gameManager = GameManager.Instance;
        levelTime = _gameManager.maxGameTime / enemyDatas.Length;
    }
    private void Update()
    {
        if (!_gameManager.isLive)
            return;
        _timer += Time.deltaTime;

        _level = Mathf.Clamp(Mathf.FloorToInt(_gameManager.gameTime / levelTime), 0, enemyDatas.Length - 1);
        if (_timer > spawnTime[_level])
        {
            EnemyType type = Random.Range(0, 100) <= 4 ? EnemyType.Elite : EnemyType.Normal; 
            Spawn(type);
            _timer = 0f;
        }
    }

    private void FixedUpdate()
    {
        transform.position = _gameManager.player.transform.position;
    }

    public void Spawn(EnemyType enemyType)
    {
        Enemy enemy = _gameManager.pool.GetEnemy();
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.Init(enemyType == EnemyType.Boss ? bossData : enemyDatas[_level], enemyType);
    }
}

