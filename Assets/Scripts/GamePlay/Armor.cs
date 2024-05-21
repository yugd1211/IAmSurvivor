using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    public ItemData.ArmorType type;

    public void Init(ItemData data)
    {
        name = "Armor " + data.itemName;
        transform.parent = GameManager.Instance.player.transform;
        transform.localPosition = Vector3.zero;

        type = data.armorType;
        ApplyArmor(data.nextDamages[0], data.nextMoveSpeed[0],data.nextWeaponSpeed[0], data.nextRate[0]);
    }

    public void LevelUp(float damage, float moveSpeed, float weaponSpeed, float weaponRate)
    {
        ApplyArmor(damage, moveSpeed, weaponSpeed, weaponRate);
    }

    private void ApplyArmor(float damage, float moveSpeed, float weaponSpeed, float weaponRate)
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
        
        foreach (Weapon weapon in weapons)
        {
            weapon.WBI.DamageMultiplier += damage;
            weapon.WBI.SpeedMultiplier += weaponSpeed;
            weapon.WBI.RateMultiplier += weaponRate;
        }
    }
}
