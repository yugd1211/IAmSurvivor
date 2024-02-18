using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health }
    public InfoType type;

    private TextMeshPro _text;
    private Slider _slider;
    private GameManager _gameManager;

    private void Start()
    {
        _text = GetComponent<TextMeshPro>();
        _slider = GetComponent<Slider>(); 
        _gameManager = GameManager.Instance;
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float currExp = _gameManager.exp;
                float maxExp = _gameManager.nextExp[_gameManager.level];
                _slider.value = currExp / maxExp;
                break;
            case InfoType.Level:
                break;
            case InfoType.Kill:
                break;
            case InfoType.Health:
                break;
            case InfoType.Time:
                break;
        }
    }

}
