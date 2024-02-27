using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Bullet : MonoBehaviour
{
    public int id;
    public float damage;
    public ItemData.WeaponType weaponType;
    public int per;
    public float speed;
    
    private Rigidbody2D _rigid;
    private SpriteRenderer _spriter;
    private Player _player;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriter = GetComponent<SpriteRenderer>();
        _player = GameManager.Instance.player;
    }

    private void FixedUpdate()
    {
        if (weaponType == ItemData.WeaponType.Melee)
        {
            if (id == 4)
            {
               transform.rotation = Quaternion.FromToRotation(Vector3.up, _player.dir);
               transform.localPosition = transform.up * 2f;
            }
            if (id == 5)
                transform.RotateAround(_player.transform.position, Vector3.forward, speed * Time.deltaTime);
        }
    }

    public void Init(float damage, int per, Vector3 dir, ItemData.WeaponType weaponType, int id, float speed)
    {
        this.id = id;
        this.damage = damage;
        this.per = per;
        this.weaponType = weaponType;
        this.speed = speed;
        if (id == 5)
        {
            _spriter.flipX = dir != Vector3.down;
        }
        if (weaponType == ItemData.WeaponType.Range)
        {
            _rigid.velocity = dir * 10;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy") || weaponType != ItemData.WeaponType.Range)
            return;
        per--;
        if (per < 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Area") || weaponType != ItemData.WeaponType.Range)
            return;
        gameObject.SetActive(false);
    }
}
