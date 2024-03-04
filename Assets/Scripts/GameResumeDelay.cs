using System.Collections;
using TMPro;
using UnityEngine;

public class GameResumeDelay : MonoBehaviour
{
    private WaitForSecondsRealtime _wait;
    private GameManager _gameManager;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _wait = new WaitForSecondsRealtime(0.5f);
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ResumeDelay()
    {
        gameObject.SetActive(true);
        StartCoroutine(GameResumeRoutine());
    }
    
    
    private IEnumerator GameResumeRoutine()
    {
        _text.text = "3";
        yield return _wait;
        _text.text = "2";
        yield return _wait;
        _text.text = "1";
        yield return _wait;
        gameObject.SetActive(false);
        _gameManager.Resume();
    }
}
