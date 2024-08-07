using UnityEngine;

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
        Orbital,
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
    public readonly static int MaxLevel = 5;
    public ItemType itemType;
    public WeaponType weaponType;
    public ArmorType armorType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Base Data")]
    public float baseDamage;
    public int baseCount;
    public float baseSpeed;
    public float baseRate;
    public int basePer;
    
    [Header("# Level Data")]
    public float[] nextDamages = new float[MaxLevel];
    public int[] nextCounts = new int[MaxLevel];
    public float[] nextWeaponSpeed = new float[MaxLevel];
    public float[] nextMoveSpeed = new float[MaxLevel];
    public float[] nextRate = new float[MaxLevel];
    public int[] nextPer = new int[MaxLevel];

    [Header("# Weapon")]
    public GameObject projectile;
    public Sprite hand;

}
