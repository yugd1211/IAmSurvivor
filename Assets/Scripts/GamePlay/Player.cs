using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public CharacterData data;
    public float health;
    public float maxHealth;
    public Vector2 InputVec { get; set; }
    public Vector2 dir;
    public float moveSpeed = 1f;
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
        health = data.HP;
        maxHealth = data.HP;
    }

    private void Start()
    {
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

    public void Damaged(float damage)
    { 
        health -= damage;
        StatisticsManager.Instance.IncrementHitCount();
        StatisticsManager.Instance.IncrementTotalHitCount();
        if (health <= 0.0f) 
            Dead();
    }

    public void Dead()
    {
        for (int i = 2; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        _anim.SetTrigger("Dead");
        _gameManager.GameOver(false);
    }

    private void FixedUpdate()
    {
        if (!_gameManager.isLive)
            return;
        Vector2 nextVec = InputVec.normalized * (data.MoveSpeed * moveSpeed * Time.fixedDeltaTime);
        _rigid.MovePosition(_rigid.position + nextVec);
    }
    
    private void LateUpdate()
    {
        if (!_gameManager.isLive)
            return;
        if (InputVec.x != 0)
            _spriter.flipX = InputVec.x < 0;
        _anim.SetFloat("Speed", InputVec.magnitude);
    }
}
