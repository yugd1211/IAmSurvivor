using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
   
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
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
        if (!_isLive || _anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;
        Move();
    }
    private void LateUpdate()
    {
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
        health = maxHealth;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Bullet") || !_isLive)
            return;

        health -= other.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());
        if (health > 0)
        {
            _anim.SetTrigger("Hit");
        }
        else
        {
            _isLive = false;
            _coll.enabled = false;
            _rigid.simulated = false;
            _spriter.sortingOrder = 1;
            _anim.SetBool("Dead", true);
            _gameManager.kill++;
            _gameManager.GetExp();
            
            // Dead();
        }
    }

    IEnumerator KnockBack()
    {
        yield return _wait;
        Vector3 playerPos = _gameManager.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        _rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }
    
    
    public void Init(SpawnData spawnData)
    {
        _anim.runtimeAnimatorController = animCon[spawnData.spriteType];
        moveSpeed = spawnData.moveSpeed;
        maxHealth = spawnData.health;
        health = spawnData.health;
    }
    
    private void Move()
    {
        Vector2 dirVec = target.position - _rigid.position;
        Vector2 nextVec = dirVec.normalized * (moveSpeed * Time.fixedDeltaTime);
        _rigid.MovePosition(_rigid.position + nextVec);
        _rigid.velocity = Vector2.zero;
    }
    
    private void Dead()
    {
        gameObject.SetActive(false);
    }
}
