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
                    weapon.speed = -150 + (-150 * rate);
                    break;
                case 1:
                    weapon.speed = 0.5f * (1f - rate);
                    break;
            }
        }
    }
    
    private void SpeedUp()
    {
        float speed = 3;
        GameManager.Instance.player.moveSpeed = speed + speed * rate;
    }
}
