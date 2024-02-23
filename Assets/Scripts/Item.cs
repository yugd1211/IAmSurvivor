using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Armor armor;

    private Image _icon;
    private TextMeshProUGUI _textLevel;
    private TextMeshProUGUI _textName;
    private TextMeshProUGUI _textDesc;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _icon = GetComponentsInChildren<Image>()[1];
        _icon.sprite = data.itemIcon;

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        _textLevel = texts[0];
        _textName = texts[1];
        _textDesc = texts[2];
        _textName.text = data.itemName;
    }

    void OnEnable()
    {
        _textLevel.text = "Lv." + level;

        switch (data.itemType)
        {
            case ItemData.ItemType.Weapon:
                _textDesc.text = string.Format(data.itemDesc, data.nextDamages[level], data.nextCounts[level]); 
                break;
            case ItemData.ItemType.Armor:
                _textDesc.text = string.Format(data.itemDesc, data.nextDamages[level]);
                break;
            default:
                _textDesc.text = string.Format(data.itemDesc); 
                break;
        }
    }
    
    public void OnClick()
    {
        //Equip(this);
        //player.Equip(Armor armor);
        switch (data.itemType)
        {
            case ItemData.ItemType.Weapon:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.nextDamages[level];
                    nextCount += data.nextCounts[level];
                    weapon.LevelUp(nextDamage, nextCount);
                }
                break;
            case ItemData.ItemType.Armor:
                if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    armor = newGear.AddComponent<Armor>();
                    armor.Init(data);
                }
                else
                {
                    float nextRate = data.nextDamages[level];
                    armor.LevelUp(nextRate);
                }
                break;
            case ItemData.ItemType.Potion:
                _gameManager.health = _gameManager.maxHealth;
                break;
        }
        if (data.itemType != ItemData.ItemType.Potion)
            level++;

        if (level == data.nextDamages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
