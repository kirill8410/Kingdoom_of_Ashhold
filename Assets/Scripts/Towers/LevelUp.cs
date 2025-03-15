using UnityEngine;

[CreateAssetMenu(fileName = "LevelUp", order = 2)]
public class LevelUp : ScriptableObject
{
    [Header("LevelUp_1")]
    public int damage_1;
    public float distance_1;
    public float attackSpeed_1;
    public int priceLevelUp_1;
    [Header("LevelUp_2")]
    public int damage_2;
    public float distance_2;
    public float attackSpeed_2;
    public int priceLevelUp_2;
}
