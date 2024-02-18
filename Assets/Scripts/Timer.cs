using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI _tmp; // TextMeshProUGUI 컴포넌트 참조를 위한 변수
    public float timeRemaining = 0f; // 타이머 시간 설정 (10초)

    void Start()
    {
        _tmp = GetComponent<TextMeshProUGUI>(); // TextMeshProUGUI 컴포넌트 가져오기
    }

    void Update()
    {
    }
}
