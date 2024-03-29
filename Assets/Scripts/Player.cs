using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
// ReSharper disable All

public class Player : MonoBehaviour
{
    public CharacterData data;
    public Vector2 InputVec { get; set; }
    public Vector2 dir;
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
        _gameManager = GameManager.Instance;
    }

    public void ChangeAnim()
    {
        _anim.runtimeAnimatorController = data.AnimCon;
    }
    

    public void OnMove(InputValue value)
    {
        InputVec = value.Get<Vector2>();
        if (InputVec == Vector2.zero)
            return;
        dir = InputVec;
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (!_gameManager.isLive || !other.transform.CompareTag("Enemy"))
            return;

        _gameManager.health -= Time.deltaTime * 10;

        if (_gameManager.health < 0.0f)
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            _anim.SetTrigger("Dead");
            _gameManager.GameOver();
        }
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
