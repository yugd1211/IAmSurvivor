using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PoolManager pool;
    public Player player;

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
            if (instance == null)
                instance = new GameManager();
            return instance;
        }
    }
}
