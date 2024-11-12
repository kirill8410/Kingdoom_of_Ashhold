using UnityEngine;

public class Tower : MonoBehaviour// Общие пораметры всех башен
{
    // Цена улучшения/покупки и уровень башни
    public int price;
    public int level;

    // Параметры атаки башни
    public float attackSpeed;
    public int damage;
    public bool isAttack; // Может ли башня атакавать

    // Дебафы
    public float attackSlowdown; // уменьшение скорости стрельбы
    public int damageReduction; // уменьшение урона

    // Бафы

    public float attackAcceleration; // Увеличение скорости стрельбы
    public int damegeIncrease; // Увеличение урона

}
