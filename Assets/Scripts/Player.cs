using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 inputVec;
    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void LateUpdate()
    {
        if (inputVec.x != 0)
            spriter.flipX = inputVec.x < 0;
        anim.SetFloat("Speed", inputVec.magnitude);
    }

    void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }
    
    public void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
}
