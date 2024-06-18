using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Weapon : MonoBehaviour
{ 
    public enum BulletType
    {
        Shovels = 0,
        Poke,
        Scythe,
        Sniper,
        MachineGun,
        Shotgun,
    }
    public WeaponInfo WI;
    private float _timer;
    private Player _player;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _player = _gameManager.player;

    }

    void Update()
    {
        if (!_gameManager.isLive)
            return;
        switch (WI.Type)
        {
            case ItemData.WeaponType.Orbital:
                transform.Rotate(Vector3.forward * (WI.Speed() * Time.deltaTime));
                break;
            case ItemData.WeaponType.Melee:
                    _timer += Time.deltaTime;

                    if (_timer >= 1 / WI.Rate())
                    {
                        _timer = 0f;
                        if (WI.ID == 4)
                            Poke();
                        else if (WI.ID == 5)
                            Scythe();
                    }
                    break;
            case ItemData.WeaponType.Range:
                _timer += Time.deltaTime;

                if (_timer >= 1 / WI.Rate())
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
        WI.Type = data.weaponType;

        WI = new WeaponInfo(data.itemId, 
            data.baseDamage * _gameManager.player.data.Damage, 
            data.baseRate, data.baseSpeed, 
            data.baseCount + _gameManager.player.data.Count, 
            data.basePer, 
            data.weaponType)
        {
            SpeedMultiplier = 1,
            DamageMultiplier = 1,
            RateMultiplier = 1,
        };

        if (WI.ID == (int)ItemData.WeaponType.Orbital)
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
        WI.DamageMultiplier += damage;
        WI.Count += count;
        WI.SpeedMultiplier += speed;
        WI.RateMultiplier += rate;
        WI.Per += per;

        if (WI.ID == (int)ItemData.WeaponType.Orbital)
            Batch();
    }

    private void Batch()
    {
        for (int i = 0; i < WI.Count; i++)
        {
            Bullet bullet;
            if (i < transform.childCount)
                bullet = transform.GetChild(i).GetComponent<Bullet>();
            else
            {
                bullet = _gameManager.pool.Get<Bullet>((int)BulletType.Shovels);
                bullet.transform.parent = transform;
            }
            Transform bulletTrans = bullet.transform;
            bulletTrans.localPosition = Vector3.zero;
            bulletTrans.localRotation = Quaternion.identity;
            bulletTrans.Rotate(Vector3.forward * (360 * i) / WI.Count);
            bulletTrans.Translate(bulletTrans.up * 1.5f, Space.World);
            bullet.Init(WI, Vector3.zero);
        }
    }
    
    private void Poke()
    {
        Bullet bullet = _gameManager.pool.Get<Bullet>((int)BulletType.Poke);
        Transform bulletTrans = bullet.transform;
        
        bulletTrans.parent = transform;
        bulletTrans.localPosition = Vector3.zero;
        bulletTrans.localScale *= 2;
        bulletTrans.rotation = Quaternion.FromToRotation(Vector3.up, _player.dir);
        bulletTrans.Translate(bulletTrans.up * 1.5f, Space.World);
        bullet.Init(WI, _player.dir);
        StartCoroutine(PokeExit(bullet));
        
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Melee);
    }    
    private void Scythe()
    {
        Vector2 dir = _player.dir;
        Bullet bullet = _gameManager.pool.Get<Bullet>((int)BulletType.Scythe);
        Transform bulletTrans = bullet.transform;
        
        bulletTrans.parent = transform;
        bulletTrans.localPosition = Vector3.zero;
        bulletTrans.localScale *= 3;
        bulletTrans.rotation = 
            Quaternion.FromToRotation(Vector3.up, dir.normalized) * 
            Quaternion.Euler(0, 0, dir == Vector2.down ? 45 : -45);
        bulletTrans.Translate(bulletTrans.up * 2f, Space.World);

        bullet.Init(WI, dir.normalized);
        
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
        if (WI.ID == 7)
        {
            for (int i = 0; i < WI.Count; i++)
            {
                Bullet bullet = _gameManager.pool.Get<Bullet>(WI.ID == 1 ? (int)BulletType.Sniper : WI.ID == 6 ? (int)BulletType.MachineGun : (int)BulletType.Shotgun);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                Quaternion q = i == 0
                    ? Quaternion.Euler(0, 0, 0)
                    : Quaternion.Euler(0, 0, i % 2 == 1 ? i * 5 : (i - 1) * -5);
                bullet.Init(WI, q * dir);
            }
        }
        else
        {
            Bullet bullet = _gameManager.pool.Get<Bullet>(WI.ID == 1 ? (int)BulletType.Sniper : WI.ID == 6 ? (int)BulletType.MachineGun : (int)BulletType.Shotgun);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bullet.Init(WI, dir);
        }
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
