using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
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
        switch (weaponType)
        {
            case ItemData.WeaponType.Orbital:
                transform.Rotate(Vector3.forward * (baseSpeed * speed * Time.deltaTime));
                break;
            case ItemData.WeaponType.Melee:
                if (id == 4)
                {
                    _timer += Time.deltaTime;

                    if (_timer >= 2 - (baseRate * (1 + rate)))
                    {
                        _timer = 0f;
                        Poke();
                    }
                }
                else if (id == 5)
                {
                    _timer += Time.deltaTime;
                    
                    if (_timer >= 2 - (baseRate * (1 + rate)))
                    {
                        _timer = 0f;
                        Scythe();
                    }
                }
                break;
            case ItemData.WeaponType.Range:
                _timer += Time.deltaTime;

                if (_timer >= 1 - (baseRate * (1 + rate)))
                {
                    _timer = 0f;
                    Fire();
                }
                break;

        }
    }

    private IEnumerator PokeExit(Bullet bullet)
    {
        yield return new WaitForSeconds(0.5f);
        bullet.transform.parent = _gameManager.pool.transform;
        bullet.transform.localScale = Vector3.one;
        bullet.gameObject.SetActive(false);
    }
    public void Init(ItemData data)
    {
        name = "Weapon " + data.itemName;
        transform.parent = _player.transform;
        transform.localPosition = Vector3.zero;
        id = data.itemId;
        weaponType = data.weaponType;
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
        if (id == (int)ItemData.WeaponType.Orbital)
            Batch();
        PlaceHandPosition(data);
    }

    private void PlaceHandPosition(ItemData data)
    {
        Hand hand;
        if (data.weaponType == ItemData.WeaponType.Melee 
            || data.weaponType == ItemData.WeaponType.Orbital)
            hand = _player.hands[0];
        else
            hand = _player.hands[1];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);
    }

    public void LevelUp(float damage, int count, float speed, float rate, int per)
    {
        this.damageMultiplier += damage;
        this.count += count;
        this.speed += speed;
        this.rate += rate;
        this.per += per;

        if (id == (int)ItemData.WeaponType.Orbital)
            Batch();
    }

    private void Batch()
    {
        for (int i = 0; i < count; i++)
        {
            Bullet bullet;
            if (i < transform.childCount)
                bullet = transform.GetChild(i).GetComponent<Bullet>();
            else
            {
                bullet = _gameManager.pool.GetMelee(0);
                bullet.transform.parent = transform;
            }
            Transform bTf = bullet.transform;
            bTf.localPosition = Vector3.zero;
            bTf.localRotation = Quaternion.identity;
            bTf.Rotate(Vector3.forward * (360 * i) / count);
            bTf.Translate(bTf.up * 1.5f, Space.World);
            bullet.Init(baseDamage * damageMultiplier, -1, Vector3.zero, weaponType, id,baseSpeed * speed);
        }
    }
    
    private void Poke()
    {
        Bullet bullet = _gameManager.pool.GetMelee(1);
        
        bullet.transform.parent = transform;
        bullet.transform.localPosition = Vector3.zero;
        bullet.transform.localScale *= 2;
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, _player.dir);
        bullet.transform.Translate(bullet.transform.up * 1.5f, Space.World);
        bullet.Init(baseDamage * damageMultiplier, count, _player.dir, weaponType, id, baseSpeed * speed);
        StartCoroutine(PokeExit(bullet));
        
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Melee);
    }    
    private void Scythe()
    {
        Vector2 dir = _player.dir;
        Bullet bullet = _gameManager.pool.GetMelee(2);
        
        bullet.transform.parent = transform;
        bullet.transform.localPosition = Vector3.zero;
        bullet.transform.localScale *= 3;
        bullet.transform.rotation = 
            Quaternion.FromToRotation(Vector3.up, dir.normalized) * 
            Quaternion.Euler(0, 0, dir == Vector2.down ? 45 : -45);
        bullet.transform.Translate(bullet.transform.up * 2f, Space.World);
        bullet.Init(baseDamage * damageMultiplier, count, dir.normalized, weaponType, id, baseSpeed * speed);
        
        StartCoroutine(PokeExit(bullet));
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Melee);
    }

    private void Fire()
    {
        if (!_player.scanner.nearestTarget)
            return;

        Vector3 targetPos = _player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Bullet bullet = _gameManager.pool.GetRange(0);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.Init(baseDamage * damageMultiplier, count, dir, weaponType, id,baseSpeed * speed);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
