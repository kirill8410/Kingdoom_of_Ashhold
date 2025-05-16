using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave")]
public class Wave : ScriptableObject
{
    public GameObject[] Enemies;
    public int[] NumberOfEnemies;
}
