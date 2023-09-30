using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies", order = 0)]
public class EnemiesData : ScriptableObject
{
    
    public new string name;
    public float health;

    public Sprite sprite;

    public float attackDamage;
    public float attackSpeed;

    public bool isRanged;
    public bool isMelee;


}
