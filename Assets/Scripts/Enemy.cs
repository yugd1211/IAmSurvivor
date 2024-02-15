using System;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D target;

    private bool _isLive = true;
    private Rigidbody2D _rigid;
    private SpriteRenderer _spriter;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriter = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
    }

    private void Move()
    {
        Vector2 dirVec = target.position - _rigid.position;
        Vector2 nextVec = dirVec.normalized * (moveSpeed * Time.fixedDeltaTime);
        _rigid.MovePosition(_rigid.position + nextVec);
        _rigid.velocity = Vector2.zero;
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
}
