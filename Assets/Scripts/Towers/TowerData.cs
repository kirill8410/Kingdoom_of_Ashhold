using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "TowerData", order = 1)]
public class TowerData : ScriptableObject
{
    public string TowerName;
    [TextArea] public string description;
    public int price;
    public GameObject tower;
}

[CreateAssetMenu(fileName = "LevelUp", menuName = "TowerData", order = 2)]
public class LevelUp : ScriptableObject
{
    [Header("LevelUp_1")]
    public int damage_1;
    public float distance_1;
    public int priceLevelUp_1;
    [Header("LevelUp_2")]
    public int damage_2;
    public float distance_2;
    public int priceLevelUp_2;
}