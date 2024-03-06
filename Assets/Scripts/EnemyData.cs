using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum EnemyType
{
    Normal = 0,
    Elite = 1,
    Boss = 2,
}
[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Object/EnemyData")]
public class EnemyData : ScriptableObject
{
    
    public int id;
    public EnemyType type;
    public float health;
    public float moveSpeed;
    public float damage;
    public bool isMelee;
    public float attackRange;
    [SerializeField] private RuntimeAnimatorController animCon;

    public RuntimeAnimatorController AnimCon => animCon;
    public GameObject projectile;
    public EnemyData(EnemyData enemyData)
    {
    }
}
