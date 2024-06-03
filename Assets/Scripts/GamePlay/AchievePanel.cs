using TMPro;
using UnityEngine;

public class AchievePanel : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;

    private void Awake()
    {
        nameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        descText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void SetName(string achieveName)
    {
        nameText.text = achieveName;
    }
    
    public void SetDesc(string description)
    {
        descText.text = description;
    }
    
}
