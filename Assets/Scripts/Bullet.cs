using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int id;
    public float damage;
    public ItemData.WeaponType weaponType;
    public int per;

    private Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir, ItemData.WeaponType weaponType)
    {
        this.damage = damage;
        this.per = per;
        this.weaponType = weaponType;
        if (per != -1)
        {
            _rigid.velocity = dir * 10;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy") || weaponType == ItemData.WeaponType.Melee)
            return;
        per--;
        if (per < 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Area") || weaponType == ItemData.WeaponType.Melee)
            return;
        gameObject.SetActive(false);
    }
}
