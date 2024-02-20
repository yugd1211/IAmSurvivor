using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class LevelUp : MonoBehaviour
{
    private GameManager _gameManager;
    private RectTransform rect;
    private Item[] _items;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        _items = GetComponentsInChildren<Item>(true);
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public void Show()
    {
        rect.localScale = Vector3.one;
        _gameManager.Stop();
    }
    public void Hide()
    {
        rect.localScale = Vector3.zero;
        _gameManager.Resume();
    }
    public void Select(int index)
    {
        _items[index].OnClick();
    }
}
