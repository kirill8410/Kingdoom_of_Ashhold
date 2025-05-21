using UnityEngine;
using UnityEngine.UIElements.Experimental;

[CreateAssetMenu(fileName = "EnemyParameters", menuName = "Parameters/EnemyParameters")]
public class EnemyParameters : ScriptableObject
{
    public string Name;

    [Space]

    public float MaxHP;
    public int Shield;
    public float Protection;
    public float Speed;
    public int DropCoins;

    [Space]

    public Enemy.EnemyTypes EnemyTypes;
    public Enemy.EnemySpell EnemySpell;

    [Space]

    public float SpellColldown;
    public float SpellDistance;
    public float SpellModifier;

}
