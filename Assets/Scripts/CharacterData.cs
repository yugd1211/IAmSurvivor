using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Object/CharacterData")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private string characterName;
    [SerializeField] private float moveSpeed;
    [SerializeField] private RuntimeAnimatorController animCon;
    [SerializeField] private float weaponSpeed;
    [SerializeField] private float weaponRate;
    [SerializeField] private float damage;
    [SerializeField] private int count;
    [SerializeField] private int initWeaponId;

    public string CharacterName => characterName;
    public float MoveSpeed => moveSpeed;
    public RuntimeAnimatorController AnimCon => animCon;
    public float WeaponSpeed => weaponSpeed;
    public float WeaponRate => weaponRate;
    public float Damage => damage;
    public int Count => count;
    public int InitWeaponId => initWeaponId;

}