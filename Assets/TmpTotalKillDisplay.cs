using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TmpTotalKillDisplay : MonoBehaviour
{
    private TextMeshProUGUI _textMeshPro;

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // _textMeshPro.text = GameManager.Instance.TotalKill.Count.ToString();
        _textMeshPro.text = StatisticsManager.Instance.GetTotalKillCount().ToString();
    }
}
