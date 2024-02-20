using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
// ReSharper disable All

public class Player : MonoBehaviour
{
    public Vector2 InputVec { get; set; }
    public float moveSpeed = 5f;
    public Scanner scanner;
    public Hand[] hands;
    
    private Rigidbody2D _rigid;
    private Animator _anim;
    private SpriteRenderer _spriter;
    private GameManager _gameManager;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriter = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }
    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public void OnMove(InputValue value)
    {
        InputVec = value.Get<Vector2>();
    }
    
    void FixedUpdate()
    {
        if (!_gameManager.isLive)
            return;
        Vector2 nextVec = InputVec.normalized * (moveSpeed * Time.fixedDeltaTime);
        _rigid.MovePosition(_rigid.position + nextVec);
    }
    void LateUpdate()
    {
        if (!_gameManager.isLive)
            return;
        if (InputVec.x != 0)
            _spriter.flipX = InputVec.x < 0;
        _anim.SetFloat("Speed", InputVec.magnitude);
    }
    
}
