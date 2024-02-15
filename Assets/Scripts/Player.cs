using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// ReSharper disable All

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D _rigid;
    private Animator _anim;
    private SpriteRenderer _spriter;

    public Vector2 InputVec { get; set; }
    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriter = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }
    
    public void OnMove(InputValue value)
    {
        InputVec = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        Vector2 nextVec = InputVec.normalized * (moveSpeed * Time.fixedDeltaTime);
        _rigid.MovePosition(_rigid.position + nextVec);
    }
    void LateUpdate()
    {
        if (InputVec.x != 0)
            _spriter.flipX = InputVec.x < 0;
        _anim.SetFloat("Speed", InputVec.magnitude);
    }
    
}
