using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIFollow : MonoBehaviour
{
    public RectTransform _rect;
    private GameManager _gameManager;
    public Transform target;
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
        if (target)
            _rect.position = Camera.main.WorldToScreenPoint(target.position);
        if (!target && _gameManager.boss)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            target = _gameManager.boss.transform;
        }
    }
}
