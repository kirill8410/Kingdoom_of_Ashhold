using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "TowerData", order = 1)]
public class TowerData : ScriptableObject
{
    public string TowerName;
    [TextArea] public string description;
    public int price;
    public GameObject tower;
}
