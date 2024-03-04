using System;
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
        if (level == 0)
            _textLevel.text = "";
        else
            _textLevel.text = "Lv." + level;

        switch (data.itemType)
        {
            case ItemData.ItemType.Weapon:            
            case ItemData.ItemType.Armor:
                int[] line =
                {
                    (int)(data.nextDamages[level] * 100),
                    data.nextCounts[level], 
                    (int)(data.nextWeaponSpeed[level] * 100),
                    (int)(data.nextMoveSpeed[level] * 100),
                    (int)(data.nextRate[level] * 100),
                };
                if (level == 0)
                    _textDesc.text = string.Format(data.itemName + "을 얻습니다!");
                else
                {
                    _textDesc.text = 
                    string.Format(data.itemDesc, 
                        data.nextDamages[level] * 100, 
                        data.nextCounts[level],
                        data.nextWeaponSpeed[level] * 100, 
                        data.nextMoveSpeed[level] * 100, 
                        data.nextRate[level] * 100);
                    string[] lines = _textDesc.text.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
                    string str = "";
                    for (int i = 0; i < 5; i++)
                    {
                        if (line[i] != 0)
                            str += lines[i] + "\n";
                    }
                    _textDesc.text = string.Format(str);

                }
                break;
            case ItemData.ItemType.Potion:
                _textDesc.text = string.Format(data.itemDesc);
                break;
            default:
                break;
        }
    }
    public void OnClick()
    {
        ObtainItem();
    }
    public void ObtainItem()
    {
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
                    weapon.LevelUp(data.nextDamages[level], 
                        data.nextCounts[level], 
                        data.nextWeaponSpeed[level], 
                        data.nextRate[level], 
                        data.nextPer[level]);
                }
                break;
            case ItemData.ItemType.Armor:
                if (level == 0)
                {
                    GameObject newArmor = new GameObject();
                    armor = newArmor.AddComponent<Armor>();
                    armor.Init(data);
                }
                else
                {
                    armor.LevelUp(data.nextDamages[level], 
                        data.nextMoveSpeed[level], 
                        data.nextWeaponSpeed[level], 
                        data.nextRate[level]);
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
