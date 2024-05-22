public class NoticeCharacter : Notice
{
    protected override void Configure<T>(T param)
    {
        if (param is CharacterType unlock)
            transform.GetChild((int)unlock).gameObject.SetActive(true);
    }
}
