using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Advertisements;
using Random = UnityEngine.Random;

public class LevelUp : MonoBehaviour
{
    private GameManager _gameManager;
    private RectTransform _rect;
    private Item[] _items;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _items = GetComponentsInChildren<Item>(true);
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public void Show()
    {
        PickRandomItem();
        _rect.localScale = Vector3.one;
        _gameManager.Stop();
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.Instance.EffectBgm(true);

    }
    public void Hide()
    {
        _rect.localScale = Vector3.zero;
        _gameManager.Resume();
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.Instance.EffectBgm(false);
    }
    public void Select(int index)
    {
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i].data.itemId != index)
                continue;
            _items[i].ObtainItem();
            break;
        }
    }

    void PickRandomItem()
    {
        // 모든 아이템 비활성화
        foreach (Item item in _items)
        {
            item.GameObject().SetActive(false);
        }
        // 랜덤 3개 아이템 활성화
        int[] ran = new int[3];
        List<int> ranList = new List<int>();

        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i].level != _items[i].data.nextDamages.Length)
                ranList.Add(i);
        }
        for (int i = 0; i < 3; i++)
        {
            int r = Random.Range(0, ranList.Count);
            ran[i] = ranList[r];
            ranList.Remove(ranList[r]);
        }
        for (int i = 0; i < ran.Length; i++)
        {
            Item ranItem = _items[ran[i]];
            ranItem.gameObject.SetActive(true);
        }
    }
}
