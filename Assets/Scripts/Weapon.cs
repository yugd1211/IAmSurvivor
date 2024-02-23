using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    
    public float baseDamage;
    public float baseRate;
    public float baseSpeed;
    
    public float damageMultiplier;
    public int count;
    public int per;
    public float speed;
    public float rate;
    public ItemData.WeaponType weaponType;
    
    private float _timer;
    private Player _player;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _player = _gameManager.player;
        speed = 1;
        damageMultiplier = 1;
    }

    void Update()
    {
        if (!_gameManager.isLive)
            return;
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.forward * (baseSpeed * speed * Time.deltaTime));
                break;
            case 1:
                _timer += Time.deltaTime;

                if (_timer >= 1 - (baseRate + rate))
                {
                    _timer = 0f;
                    Fire();
                }
                break;
            default:
                break;
        }
    }

    public void Init(ItemData data)
    {
        name = "Weapon " + data.itemName;
        transform.parent = _player.transform;
        transform.localPosition = Vector3.zero;
        weaponType = data.weaponType;
        id = data.itemId;
        baseDamage = data.baseDamage * _gameManager.player.data.Damage;
        count = data.baseCount + _gameManager.player.data.Count;
        baseRate = data.baseRate;
        baseSpeed = data.baseSpeed;

        for (int i = 0; i < _gameManager.pool.prefabs.Length; i++)
        {
            if (data.projectile == _gameManager.pool.prefabs[i])
            {
                prefabId = i;
            }
        }
        Hand hand = _player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);
        Batch();
        // _player.BroadcastMessage("ApplyArmor", SendMessageOptions.DontRequireReceiver);
    }

    public void LevelUp(float damage, int count, float speed, float rate, int per)
    {
        this.damageMultiplier += damage;
        this.count += count;
        this.speed += speed;
        this.rate += rate;
        this.per += per;

        if (id == 0)
            Batch();
        // _player.BroadcastMessage("ApplyArmor", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;
            if (i < transform.childCount)
                bullet = transform.GetChild(i);
            else
            {
                bullet = _gameManager.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;
            bullet.Rotate(Vector3.forward * (360 * i) / count);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(baseDamage * damageMultiplier, -1, Vector3.zero, weaponType);
        }
    }

    void Fire()
    {
        if (!_player.scanner.nearestTarget)
            return;

        Vector3 targetPos = _player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;
        
        Transform bullet = _gameManager.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(baseDamage * damageMultiplier, count, dir, weaponType);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
