using UnityEngine;

[CreateAssetMenu(fileName = "TowerParameters", menuName = "Parameters/TowerParameters")]
public class TowerParameters : ScriptableObject
{
    public string TowerName;
    [TextArea] public string TowerDescription;

    [Header("Level 1")]
    public float Damage_1;
    public float AttackSpeed_1;
    public float AttackDistance_1;
    public int Price_1;

    [Header("Level 2")]
    public float Damage_2;
    public float AttackSpeed_2;
    public float AttackDistance_2;
    public int Price_2;

    [Header("Level 3")]
    public float Damage_3;
    public float AttackSpeed_3;
    public float AttackDistance_3;
    public int Price_3;

    [Space]

    public Tower.DamageTypes DamageType;
    public Tower.TowerTypes TowerType;
}