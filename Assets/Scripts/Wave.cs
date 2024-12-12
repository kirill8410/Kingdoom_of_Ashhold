using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave")]
public class Wave : ScriptableObject
{
    public GameObject[] Enemies;
    public int[] NumberOfEnemies;

    public void SpawnEnemies()
    {

    }
}
