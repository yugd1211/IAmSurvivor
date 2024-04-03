public class NoticeCharacter : Notice
{
    protected override void Configure<T>(T param)
    {
        if (param is AchieveManager.Unlock unlock)
            transform.GetChild((int)unlock).gameObject.SetActive(true);
    }
}
