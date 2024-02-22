using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public RectTransform _rect;
    private GameManager _gameManager;
    void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }
    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    void FixedUpdate()
    {
        // _rect.position = Vector3.forward;
        _rect.position = Camera.main.WorldToScreenPoint(_gameManager.player.transform.position);
    }
}
