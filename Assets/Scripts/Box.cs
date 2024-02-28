using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Box : MonoBehaviour
{
    public GameObject[] prefabs;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        GameObject go = Instantiate(prefabs[Random.Range(0, prefabs.Length)]);
        go.transform.position = GameManager.Instance.player.dir * 1;
        Exp exp = go.GetComponent<Exp>(); 
        if (exp)
        {
            exp.Init(Random.Range(0, 3) switch
            {
                0 => Exp.ExpType.Normal,
                1 => Exp.ExpType.Elite,
                _ => Exp.ExpType.Boss,
            });
        }
        gameObject.SetActive(false);
    }
}
