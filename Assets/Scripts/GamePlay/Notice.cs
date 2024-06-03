using System.Collections;
using UnityEngine;

public abstract class Notice : MonoBehaviour
{
    protected abstract void Configure<T>(T param);

    public void Init<T>(T param)
    {
        Configure(param);
    }
    
    public void DisplayNotice()
    {
        StartCoroutine(ShowForSeconds(5));
    }

    private IEnumerator ShowForSeconds(int second)
    {
        gameObject.SetActive(true);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.LevelUp);
        yield return new WaitForSecondsRealtime(second);
        gameObject.SetActive(false);
    }
}
