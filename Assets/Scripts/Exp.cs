using System;
using UnityEngine;

public class Exp : MonoBehaviour
{
    public enum ExpType
    {
        Normal = 0,
        Elite = 1,
        Boss = 2,
    }

    public Sprite[] sprite;
    private ExpType _type;
    private SpriteRenderer _spriter;
    private Transform _target = null;
    private GameManager _gameManager;
    private float speed = 10.0f; // 움직일 속도


    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _spriter = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _target = null;
    }

    private void FixedUpdate()
    {
        if (!_gameManager.isLive)
            return;
        if (!_target)
            return;
        Move();
    }
    
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.fixedDeltaTime);
    }
    

    public void Init(ExpType type)
    {
        _type = type;
        _spriter.sprite = sprite[(int)_type];
    }

    public void Follow(Transform target)
    {
        _target = target;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            _gameManager.GetExp((int)_type * 2 + 1);
            
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Select);
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("AreaMagnet"))
            Follow(other.transform);

    }
}
