using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public EnemyData data;
    private EnemyType _type;
   
    public float health;
    public Rigidbody2D target;

    private bool _isLive;
    private Rigidbody2D _rigid;
    private Collider2D _coll;
    private SpriteRenderer _spriter;
    private Animator _anim;
    private WaitForFixedUpdate _wait;
    private GameManager _gameManager;

    private void Awake()
    {
        _wait = new WaitForFixedUpdate();
        _anim = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>();
        _spriter = GetComponent<SpriteRenderer>();
        _gameManager = GameManager.Instance;
    }
    
    private void FixedUpdate()
    {
        if (!_gameManager.isLive)
            return;
        if (!_isLive || _anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;
        Move();
    }
    private void LateUpdate()
    {
        if (!_gameManager.isLive)
            return;
        if (!_isLive)
            return;
        _spriter.flipX = target.position.x < _rigid.position.x;
    }
    private void OnEnable()
    {
        target = _gameManager.player.GetComponent<Rigidbody2D>();
        _isLive = true;
        _coll.enabled = true;
        _rigid.simulated = true;
        _spriter.sortingOrder = 2;
        _anim.SetBool("Dead", false);
    }

    private void OnDisable()
    {
        if (_type != EnemyType.Boss)
            return;
        _gameManager.GameVictory();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Bullet") || !_isLive)
            return;

        health -= other.GetComponent<Bullet>().WeaponInfo.Damage();
        StartCoroutine(KnockBack());
        if (health > 0)
        {
            _anim.SetTrigger("Hit");
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            _isLive = false;
            _coll.enabled = false;
            _rigid.simulated = false;
            _spriter.sortingOrder = 1;
            _anim.SetBool("Dead", true);
            _gameManager.kill++;
            if (_type == EnemyType.Normal)
            {
                Exp exp = _gameManager.pool.GetExp(0);
                exp.Init((Exp.ExpType)_type);
                exp.transform.position = transform.position;
            }
            if (Random.Range(0, 100) < 5)
            {
                Box box = _gameManager.pool.GetBox(0);
                box.transform.position = transform.position;
                box.gameObject.SetActive(true);
            }
            if (_gameManager.isLive)
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }

    IEnumerator KnockBack()
    {
        yield return _wait;
        Vector3 playerPos = _gameManager.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        _rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }
    
    
    public void Init(EnemyData enemyData, EnemyType enemyType)
    {
        data = enemyData;
        health = data.health;
        _anim.runtimeAnimatorController = data.AnimCon;
        _type = enemyType; 
        if (_type == EnemyType.Elite)
        {
            transform.localScale *= 1.5f;
            _spriter.color = Color.red;

            health *= 10;
        }
        else if (_type == EnemyType.Boss)
            transform.localScale *= 3f;
    }
    
    private void Move()
    {
        Vector2 dirVec = target.position - _rigid.position;
        Vector2 nextVec = dirVec.normalized * (data.moveSpeed * Time.fixedDeltaTime);
        _rigid.MovePosition(_rigid.position + nextVec);
        _rigid.velocity = Vector2.zero;
    }
    
    private void Dead()
    {
        gameObject.SetActive(false);
    }
}
