using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    
    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime = 20f;

    [Header("# Player Info")]
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = {3, 5, 10, 30, 60, 100, 150, 210, 280, 360, 460, 600};
    
    public void Awake()
    {
        pool = FindObjectOfType<PoolManager>();
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    public static GameManager Instance
    {
        get
        {
            if (!instance)
                instance = new GameManager();
            return instance;
        }
    }

    private void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }
    public void GetExp()
    {
        exp++;

        if (exp >= nextExp[level])
        {
            exp -= nextExp[level];
            level++;
        }
    }
}
