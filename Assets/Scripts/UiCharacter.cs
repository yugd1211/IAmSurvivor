using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCharacter : MonoBehaviour
{
    public CharacterData data;

    private TextMeshProUGUI _text;
    private Button _button;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _button = GetComponentInChildren<Button>();
        _text.text = data.CharacterName;
        _button.onClick.AddListener(() =>
        {
            GameManager.Instance.player.data = data;
            GameManager.Instance.GameStart();
        });
    }
    
    
    
}
