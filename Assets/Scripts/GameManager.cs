using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PoolManager pool;
    public Player player;
    public float gameTime;
    public float maxGameTime = 20f;

    private static GameManager instance;
    
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
}
