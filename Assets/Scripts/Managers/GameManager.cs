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

    private Coroutine _coroutine; 

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
        Application.targetFrameRate = 60;
        DataManager.Init();
        KillManager = new KillManager(0, DataManager.LoadPlayLog().KillCount);
        StatisticsManager.Instance.InitPlayLog();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 게임이 끝날때 코루틴 함수를 호출한다. 문제는 게임매니저는 DontDestroyOnLoad 객체이기 때문에 씬의 이동시에도 코루틴 함수가 진행된다.
        // 그렇기 때문에 게임 끝날때의 코루틴 함수가 메인씬에서 혹은 그 다음 다시 시작하는 게임씬에서도 계속 호출될수 있기때문에 씬이동 시 해당 코루틴을 종료시켜준다.
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        if (scene.name == "GameScene")
            GameStart();        
    }

    private void GameStart()
    {
        StatisticsManager.Instance.InitInGameData();
        gameTime = 0;
        level = 0;
        exp = 0;
        player = FindObjectOfType<Player>();
        uiLevelUp = FindObjectOfType<LevelUp>();
        uiResult = FindObjectOfType<Result>(true);
        pool = PoolManager.Instance;
        
        player.data = data;
        player.ChangeAnim();
        uiLevelUp.Select(player.data.InitWeaponId);
        
        Resume();
        AudioManager.Instance.PlayBgm(true);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Select);
    }
    public void GoToMainScene()
    {
        pool.ExecuteMethodOnAllObjects("DestroyAll");
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
            boss = FindObjectOfType<Spawner>().Spawn(EnemyType.Boss);
        
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
        pool.ExecuteMethodOnAllObjects("DisableAll");
        if (isWin)
            StatisticsManager.Instance.IncrementVictoryCount();
        else
            StatisticsManager.Instance.IncrementDefeatCount();
        DataManager.SavePlayLog();
        _coroutine = StartCoroutine(GameEndRoutine(isWin));
    }
    
    private IEnumerator GameEndRoutine(bool isWin)
    {
        isLive = false;
        isEnd = false;
        uiResult.gameObject.SetActive(true);
        uiResult.GameOver(isWin);
        AudioManager.Instance.PlayBgm(false);
        AudioManager.Instance.PlaySfx(isWin ? AudioManager.Sfx.Win : AudioManager.Sfx.Lose);
        VictoryAction?.Invoke();
        DefeatAction?.Invoke();
        yield return new WaitForSeconds(5f);
        Pause();
    }
}