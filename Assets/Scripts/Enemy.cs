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
    private SpriteRenderer _spriter;
    private Animator _anim;
    private WaitForFixedUpdate _wait;
    private readonly static int Hit = Animator.StringToHash("Hit");

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriter = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _wait = new WaitForFixedUpdate();
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
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        _isLive = true;
        health = maxHealth;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Bullet"))
            return;

        health -= other.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());
        if (health > 0)
        {
            _anim.SetTrigger(Hit);
        }
        else
        {
            Dead();
        }
    }

    IEnumerator KnockBack()
    {
        yield return _wait;
        Vector3 playerPos = GameManager.Instance.player.transform.position;
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
