using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave")]
public class Wave : ScriptableObject
{
    public Enemy[] Enemies;
    public int[] NumberOfEnemies;

    public IEnumerator SpawnEnemies(LevelManager LM)
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < Enemies.Length; i++)
        {
            if (NumberOfEnemies[i] == 0)
            {
                NumberOfEnemies[i] = 1;
            }
            for (int j = 0; j < NumberOfEnemies[i]; j++)
            {
                GameObject enemy = Instantiate(Enemies[i].gameObject, LM.enemySpawn.position, LM.enemySpawn.rotation);
                enemy.GetComponent<Enemy>().points = LM.points; 
                yield return new WaitForSeconds(0.5f);
            }
        }
        LM.StopSpawn();
    }
}
