using TMPro;
public class NoticeAchieve : Notice
{
    protected override void Configure<T>(T param)
    {
        if (param is Achieve achieve)
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = achieve.name;
    }
}
