using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave")]
public class Wave : ScriptableObject
{
    public Enemy[] Enemies;
    public int[] NumberOfEnemies;
}
