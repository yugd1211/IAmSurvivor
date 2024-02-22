using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        name = "Gear " + data.itemName;
        transform.parent = GameManager.Instance.player.transform;
        transform.localPosition = Vector3.zero;

        type = data.itemType;
        rate = data.nextDamages[0];
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    private void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove :
                RateUp();
                break;
            case ItemData.ItemType.Shoe :
                SpeedUp();
                break;
        }
    }

    private void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0:
                    float speed = 150 * GameManager.Instance.player.data.WeaponSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;
                case 1:
                    speed = 0.5f * GameManager.Instance.player.data.WeaponRate;
                    weapon.speed = speed * (1f - rate);
                    break;
            }
        }
    }
    
    private void SpeedUp()
    {
        float speed = GameManager.Instance.player.moveSpeed;
        GameManager.Instance.player.moveSpeed = speed * rate;
    }
}
