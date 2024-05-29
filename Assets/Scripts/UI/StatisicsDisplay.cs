using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatisicsDisplay : MonoBehaviour
{
	public TextMeshProUGUI kill;
	public TextMeshProUGUI hit;
	public TextMeshProUGUI victory;
	public TextMeshProUGUI defeat;

	private void Start()
	{
		kill.text = $"처치 횟수 : {StatisticsManager.Instance.GetTotalKillCount()}";
		hit.text = $"피격 횟수 : {StatisticsManager.Instance.GetTotalHitCount()}";
		victory.text = $"생존 횟수 : {StatisticsManager.Instance.GetVictoryCount()}";
		defeat.text = $"사망 횟수 : {StatisticsManager.Instance.GetDefeatCount()}";
	}
}
