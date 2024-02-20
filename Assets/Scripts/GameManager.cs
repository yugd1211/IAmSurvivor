using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    
    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime = 20f;
    public bool isLive;

    [Header("# Player Info")]
    public int playerId;
    public int level = 0;
    public int kill;
    public int exp;
    public float health;
    public float maxHealth;
    public int[] nextExp;
    
    public void Awake()
    {
        pool = FindObjectOfType<PoolManager>();
        if (_instance != null)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;
        
        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId % 2); // tmp
        Resume();
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
            isLive = false;
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }
    public void GetExp()
    {
        if (!isLive)
            return;
        exp++;
        if (exp >= nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            exp -= nextExp[level];
            level++;
            uiLevelUp.Show();
        }
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
