using System.Collections;
using System.IO;
using UnityEngine;

public class Tower : MonoBehaviour// Общие пораметры всех башен
{
    [Header("Attack")]
    public float attackDistance;
    public float attackSpeed;
    public int damage;
    public enum DamageTypes
    {
        Physical, Magic
    }
    public DamageTypes damageType;
    public bool isAttack = true; // Может ли башня атакавать

    public GameObject attackPrefab; // Объект которым стреляет башня
    public GameObject Distance;
    public Transform attackPoint; // Точка появления снаряда

    private void Update()
    {
        Distance.transform.localScale = new Vector3(attackDistance * 2f, attackDistance * 2f, 1f);
    }
}

public interface TowerFunctions // Общие функции башен
{
    public IEnumerator Attack(); // Атака башни 
    public IEnumerator SearchTarget(); // Поиск цели
    public void LevelUp(); // Увеничение параметров при повышении уровня
    public int PriceLevelUp { get; set; }
    public int Towerlevel { get; set; }
    public LevelUp levelUp { get; set; }
}
