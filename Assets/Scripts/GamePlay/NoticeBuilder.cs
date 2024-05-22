using UnityEngine;

public class NoticeBuilder : MonoBehaviour
{
    private WaitForSecondsRealtime _wait = new WaitForSecondsRealtime(5);
    public GameObject achievePrefab;
    public GameObject characterPrefab;
    private Notice _notice;

    public void BuildNotice(CharacterType characterType)
    {
        GameObject noticeObject = Instantiate(characterPrefab, gameObject.transform);
        Notice notice = noticeObject.GetComponent<Notice>(); 
        notice.Init(characterType);
        notice.DisplayNotice();
    }
    
    public void BuildNotice(Achieve achieve)
    {
        GameObject noticeObject = Instantiate(achievePrefab, gameObject.transform);
        Notice notice = noticeObject.GetComponent<Notice>();
        notice.Init(achieve);
        notice.DisplayNotice();
    }
}