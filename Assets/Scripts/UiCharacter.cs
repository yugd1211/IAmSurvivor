using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiCharacter : MonoBehaviour
{
    public CharacterData data;

    private GameManager _gameManager;
    private TextMeshProUGUI _text;
    private Button _button;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _button = GetComponentInChildren<Button>();
        _gameManager = GameManager.Instance; 
        _text.text = data.CharacterName;
        _button.onClick.AddListener(() =>
        {
            _gameManager.data = data;
            SceneManager.LoadScene(1);
        });
    }

    // 씬 로드 후에 호출되는 메서드입니다.

}
