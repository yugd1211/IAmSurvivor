using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType
    {
        Exp,
        Level,
        Kill, 
        Time,
        Health,
    }
    
    public InfoType type;

    private TextMeshProUGUI _text;
    private Slider _slider;
    private GameManager _gameManager;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
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
                _text.text = $"Lv.{_gameManager.level:F0}";
                break;
            case InfoType.Kill:
                _text.text = $"{_gameManager.kill:F0}";
                break;
            case InfoType.Health:
                float currHealth = _gameManager.health;
                float maxHealth = _gameManager.maxHealth;
                _slider.value = currHealth / maxHealth;
                break;
            case InfoType.Time:
                _text.text = $"{Mathf.FloorToInt(_gameManager.gameTime / 60 % 60):00}:{Mathf.FloorToInt(_gameManager.gameTime % 60):00}";
                break;
        }
    }

}
