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
        timeRemaining += Time.deltaTime; // 매 프레임마다 경과 시간만큼 타이머를 감소시킵니다.
        _tmp.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timeRemaining % 60), Mathf.FloorToInt((timeRemaining * 100) % 100));
    }
}
