using UnityEngine;

public class Enemy : MonoBehaviour // Общий скрипт для всех врагов
{
    // Здоровье врага
    public int HP;
    public int maxHP;

    // Защита врага
    public int protection;
    public Tower.DamageTypes protectionType;

    // Тип врага
    public EnemyTypes enemyType;
    public enum EnemyTypes
    {
        None, Boss
    }

    // Скорость и тип движения врага
    public float speed;
    public MoveTypes moveType;
    public enum MoveTypes
    {
        Walk/*По земле*/, Fly/*По воздуху*/
    }

    // Точки движения врага
    public GameObject[] points;
    private int numberPoint = 0;
    private GameObject point;

    public float distanceToPoint; // дистанция до следующей точки

    private void Start()
    {
        maxHP = HP;
        point = points[numberPoint];
    }

    private void Update()
    {
        // Определение дистанции до точки
        distanceToPoint = Vector3.Distance(point.transform.position, gameObject.transform.position);
        // Переключение на следующую точку
        if (distanceToPoint < 0.1f )
        {
            numberPoint += 1;
            point = points[numberPoint];
        }
    }
}
