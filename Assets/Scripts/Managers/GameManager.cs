using System;
using System.Collections;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameManager : Singleton<GameManager>
{
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public Enemy boss;
    public KillManager KillManager;
    public Action VictoryAction;
    public Action DefeatAction;
    
    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime = 20f;
    public bool isLive;
    public bool isEnd;

    [Header("# Player Info")]
    public CharacterData data;
    public int level;
    public int exp;
    public int[] nextExp;

    protected override void AwakeInit()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Application.targetFrameRate = 60;
        DataManager.Init();
        KillManager = new KillManager(0, DataManager.LoadPlayLog().KillCount);
        StatisticsManager.Instance.InitPlayLog();
    }

    private void Start()
    {
        pool = FindObjectOfType<PoolManager>();
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
            GameStart();        
    }

    public void GameStart()
    {
        StatisticsManager.Instance.InitInGameData();
        gameTime = 0;
        level = 0;
        exp = 0;
        player = FindObjectOfType<Player>();
        uiLevelUp = FindObjectOfType<LevelUp>();
        pool = FindObjectOfType<PoolManager>();
        uiResult = FindObjectOfType<Result>(true);
        
        player.data = data;
        player.ChangeAnim();
        uiLevelUp.Select(player.data.InitWeaponId);
        
        Resume();
        AudioManager.Instance.PlayBgm(true);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Select);
    }
    public void GoToMainScene()
    {
        pool.DestroyAllObjects();
        SceneManager.LoadScene("MainScene");
    }
    
    private void Update()
    {
        if (!isLive)
            return;

        if (gameTime >= maxGameTime)
            gameTime = maxGameTime;
        else
            gameTime += Time.deltaTime;

        if (!boss && gameTime >= maxGameTime)
        {
            boss = FindObjectOfType<Spawner>().Spawn(EnemyType.Boss);
        }
        if (level < nextExp.Length && exp >= nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            exp -= nextExp[level];
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
    public void Pause()
    {
        isLive = false;
        Time.timeScale = 0;
    }
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }

    public void GameExit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
    
}


/// <summary>
/// coroutine function
/// </summary>
public partial class GameManager
{
    public void GameOver(bool isWin)
    {
        pool.DisableAllObjects();
        if (isWin)
            StatisticsManager.Instance.IncrementVictoryCount();
        else
            StatisticsManager.Instance.IncrementDefeatCount();
        DataManager.SavePlayLog();
        StartCoroutine(GameEndRoutine(isWin));
    }
    
    private IEnumerator GameEndRoutine(bool isWin)
    {
        isLive = false;
        isEnd = false;
        VictoryAction?.Invoke();
        DefeatAction?.Invoke();
        uiResult.gameObject.SetActive(true);
        uiResult.GameOver(isWin);
        AudioManager.Instance.PlayBgm(false);
        AudioManager.Instance.PlaySfx(isWin ? AudioManager.Sfx.Win : AudioManager.Sfx.Lose);
        yield return new WaitForSeconds(5f);
        // Pause();
    }
}


