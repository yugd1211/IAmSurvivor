using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private GameManager _gameManager;
    private Transform _target = null;
    public int speed = 10;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
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
    private void Follow(Transform target)
    {
        _target = target;
    }
    
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.fixedDeltaTime);
    }

    private void PullExp(Transform target)
    {
        List<Exp> exps = _gameManager.pool.GetAll<Exp>();
        foreach (Exp item in exps)
        {
            item.Follow(target);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("Player"))
            return;
        PullExp(other.transform);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Select);
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AreaMagnet"))
            Follow(other.transform);
    }
}
