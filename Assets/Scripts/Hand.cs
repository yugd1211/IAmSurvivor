using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;

    private SpriteRenderer player;

    private Vector3 _rightPos = new Vector3(0.35f, -0.15f, 0); 
    private Vector3 _rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
    private Quaternion _leftRot = Quaternion.Euler(0,0, -35);
    private Quaternion _leftRotReverse = Quaternion.Euler(0,0, -135);

    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    private void LateUpdate()
    {
        bool isReverse = player.flipX;

        if (isLeft)
        {
            // 근접무기
            transform.localRotation = isReverse ? _leftRotReverse : _leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 99 : 101;
        }
        else
        {
            // 원거리무기
            transform.localPosition = isReverse ? _rightPosReverse : _rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 101 : 99;
        }
    }
}
