using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour// Общие пораметры всех башен
{
    // Цена улучшения/покупки и уровень башни
    public int price;
    public int level = 1;

    // Параметры атаки башни
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
    public Transform attackPoint; // Точка появления снаряда

    // Дебафы
    public int damageReduction; // уменьшение урона

    // Бафы
    public int damegeIncrease; // Увеличение урона
    // Бонусы знамён
    public bool isFireBoost;
    public bool isIceBoost;
    public bool isWindBoost;
    public bool isHeroBoost;
}

public interface TowerFunctions // Общие функции башен
{
    public IEnumerator Attack(); // Атака башни 
    public IEnumerator SearchTarget(); // Поиск цели
    public void LevelUp(); // Увеничение параметров при повышении уровня
}
