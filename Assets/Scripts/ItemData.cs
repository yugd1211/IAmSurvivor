using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        None,
        Weapon, 
        Armor, 
        Potion
    }
    
    public enum WeaponType
    {
        None,
        Melee, 
        Range,
    }
    public enum ArmorType
    {
        None,
        Glove, 
        Shoe, 
    }
    
    [Header("# Main Info")]
    public ItemType itemType;
    public WeaponType weaponType;
    public ArmorType armorType;
    
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;
    public float[] nextDamages;
    public int[] nextCounts;

    [Header("# Weapon")]
    public GameObject projectile;
    public Sprite hand;

}
