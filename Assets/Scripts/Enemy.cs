using System;
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
    private SpriteRenderer _spriter;
    private Animator _anim;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriter = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (!_isLive)
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
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        _isLive = true;
        health = maxHealth;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Bullet"))
            return;

        health -= other.GetComponent<Bullet>().damage;

        if (health > 0)
        {
        }
        else
        {
            Dead();
        }
    }

    private void Dead()
    {
        gameObject.SetActive(false);
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
}
