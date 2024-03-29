using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public GameObject[] titles;
    private Button _button;

    private void Awake()
    {
        _button = GetComponentInChildren<Button>(true);
        _button.onClick.AddListener(() => GameManager.Instance.GameRetry());
    }

    public void Lose()
    {
        titles[0].SetActive(true);
    }
    
    public void Win()
    {
        titles[1].SetActive(true);
    }
}
