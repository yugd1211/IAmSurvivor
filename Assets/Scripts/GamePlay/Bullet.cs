using System;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Bullet : MonoBehaviour
{
    // public int id;
    // public float damage;
    // public ItemData.WeaponType WeaponInfo.type;
    // public int per;
    // public float speed;
    public WeaponInfo WeaponInfo;
    
    private Rigidbody2D _rigid;
    private SpriteRenderer _spriter;
    private Player _player;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriter = GetComponent<SpriteRenderer>();
        // _player = GameManager.Instance.player;
    }

    private void OnEnable()
    {
        _player = GameManager.Instance.player;
    }


    private void FixedUpdate()
    {
        if (WeaponInfo.Type == ItemData.WeaponType.Melee)
        {
            if (WeaponInfo.ID == 4)
            {
                transform.rotation = Quaternion.FromToRotation(Vector3.up, _player.dir);
                transform.localPosition = transform.up * 2f;
            }
            if (WeaponInfo.ID == 5)
                transform.RotateAround(_player.transform.position, Vector3.forward, WeaponInfo.Speed() * Time.deltaTime);
        }
    }

    public void Init(WeaponInfo info, Vector3 dir)
    {
        WeaponInfo = info;
        if (WeaponInfo.ID == 5)
        {
            _spriter.flipX = dir != Vector3.down;
        }
        if (WeaponInfo.Type == ItemData.WeaponType.Range)
        {
            _rigid.velocity = dir * 10;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy") || WeaponInfo.Type != ItemData.WeaponType.Range)
            return;
        WeaponInfo.Per--;
        if (WeaponInfo.Per < 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Area") || WeaponInfo.Type != ItemData.WeaponType.Range)
            return;
        gameObject.SetActive(false);
    }
}
