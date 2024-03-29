using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance => _instance ? _instance : null;
    
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyClearner;
    public Enemy boss;
    
    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime = 20f;
    public bool isLive;
    public bool isBoss;

    [Header("# Player Info")]
    public CharacterData data;
    public int level = 0;
    public int kill;
    public int exp;
    public float health;
    public float maxHealth;
    public int[] nextExp;
    
    public void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(this.gameObject);
        if (_instance != null)
            Destroy(this.gameObject);
        else
            _instance = this;
        
        Application.targetFrameRate = 60;
        pool = FindObjectOfType<PoolManager>();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
            GameStart();        
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void GameStart()
    {
        health = maxHealth;

        gameTime = 0;
        level = 0;
        kill = 0;
        exp = 0;
        isBoss = false;
        health = maxHealth;
        player = FindObjectOfType<Player>();
        uiLevelUp = FindObjectOfType<LevelUp>();
        pool = FindObjectOfType<PoolManager>();
        enemyClearner = FindObjectOfType<Bullet>(true).gameObject;
        uiResult = FindObjectOfType<Result>(true);
        
        player.data = data;
        player.ChangeAnim();
        uiLevelUp.Select(player.data.InitWeaponId);
        

        Resume();
        AudioManager.Instance.PlayBgm(true);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.Instance.PlayBgm(false);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Lose);
    }
    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    private IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyClearner.SetActive(true);
        
        yield return new WaitForSeconds(0.5f);
        
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.Instance.PlayBgm(false);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Win);

    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
    
    private void Update()
    {
        if (!isLive)
            return;

        if (gameTime >= maxGameTime)
            gameTime = maxGameTime;
        else
            gameTime += Time.deltaTime;

        if (!isBoss && gameTime >= maxGameTime)
        {
            isBoss = true;
            boss = FindObjectOfType<Spawner>().Spawn(EnemyType.Boss);
        }
        if (level < nextExp.Length && exp >= nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            this.exp -= nextExp[level];
            level++;
            uiLevelUp.Show();
        }
    }
    public void GetExp(int exp)
    {
        if (!isLive)
            return;
        this.exp += exp;
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
