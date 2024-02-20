using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class LevelUp : MonoBehaviour
{
    private RectTransform rect;
    private Item[] _items;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        _items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        rect.localScale = Vector3.one;
    }
    public void Hide()
    {
        rect.localScale = Vector3.zero;
    }
    public void Select(int index)
    {
        _items[index].OnClick();
    }
}
