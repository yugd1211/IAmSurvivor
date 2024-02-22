using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;
    
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
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.forward * (speed * Time.deltaTime));
                break;
            case 1:
                _timer += Time.deltaTime;

                if (_timer >= speed)
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

        id = data.itemId;
        damage = data.baseDamage * _gameManager.player.data.Damage;
        count = data.baseCount + _gameManager.player.data.Count;

        for (int i = 0; i < _gameManager.pool.prefabs.Length; i++)
        {
            if (data.projectile == _gameManager.pool.prefabs[i])
            {
                prefabId = i;
            }
        }
        
        switch (id)
        {
            case 0:
                speed = -150 * _gameManager.player.data.WeaponSpeed;
                Batch();
                break;
            case 1:
                speed = (1 - 0.5f) * _gameManager.player.data.WeaponRate;
                break;
            default:
                break;
        }
        
        Hand hand = _player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);
        _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage * _gameManager.player.data.Damage;
        this.count += count;

        if (id == 0)
            Batch();
        
        _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
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
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
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
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
