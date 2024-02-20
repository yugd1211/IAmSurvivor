using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance => _instance ? _instance : null;
    
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    
    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime = 20f;
    public bool isLive;

    [Header("# Player Info")]
    public int level = 0;
    public int kill;
    public int exp;
    public int health;
    public int maxHealth;
    public int[] nextExp;
    
    public void Awake()
    {
        pool = FindObjectOfType<PoolManager>();
        if (_instance != null)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    public void GameStart()
    {
        health = maxHealth;
        
        uiLevelUp.Select(0); // tmp
        isLive = true;
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
        }
    }
    public void GetExp()
    {
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
