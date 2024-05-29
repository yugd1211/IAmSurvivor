using System;
using System.Collections;
using Cinemachine;
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
    private float _attackCoolTime;

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
        _attackCoolTime += Time.fixedDeltaTime;
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

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!_gameManager.isLive || !other.transform.CompareTag("Player"))
            return;
        if (_attackCoolTime < data.coolTime)
            return;
        _attackCoolTime = 0;
        Attack(other.gameObject.GetComponent<Player>());
    }

    public void Attack(Player player)
    {
        player.TakeDamage(data.damage);
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
        _gameManager.GameEnd(true);
    }

    public void Attacked(float damage)
    {
        health -= damage;
        StartCoroutine(KnockBack());
        if (health > 0)
        {
            _anim.SetTrigger("Hit");
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            Die(true);
        }
    }


    public void Die(bool killCount)
    {
        if (!_isLive)
            return;
        _isLive = false;
        _coll.enabled = false;
        _rigid.simulated = false;
        _spriter.sortingOrder = 1;
        _anim.SetBool("Dead", true);
        if (killCount)
        {
            KillManager.Instance.IncrementKillCount();
        }
        if (Random.Range(0, 100) < 5)
        {
            Box box = _gameManager.pool.GetBox(0);
            box.transform.position = transform.position;
            box.gameObject.SetActive(true);
        }
        else
        {
            Exp exp = _gameManager.pool.GetExp(0);
            exp.Init((Exp.ExpType)_type - 1);   
            exp.transform.position = transform.position;
        }
        if (_gameManager.isLive)
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Dead);
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
        transform.localScale = Vector3.one;
        _spriter.color = Color.white;
        gameObject.SetActive(false);
    }
}
