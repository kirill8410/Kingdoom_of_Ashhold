using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour // Общий скрипт для всех врагов
{
    private NavMeshAgent agent; // NavMeshAgent

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
    public int numberPoint = 0;
    private Vector3 point;

    public float distanceToPoint; // дистанция до следующей точки

    private void Start()
    {
        maxHP = HP;
        point = points[numberPoint].transform.position;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Определение дистанции до точки
        distanceToPoint = Vector3.Distance(point, gameObject.transform.position);
        // Переключение на следующую точку
        if (distanceToPoint < 0.3f)
        {
            numberPoint += 1;
            if (numberPoint >= points.Length)
            {
                Finish();
            }
            else
            {
                point = points[numberPoint].transform.position;
            }
        }
        // NawMesh
        agent.speed = speed;
        agent.destination = point;
       
        if (HP <= 0)
        {
            Dead();
        }
    }

    private void Finish() // Действия врага когда он дошёл до конца
    {
        Destroy(gameObject);
    }

    private void Dead() // Действия врага при смерти
    {
        Destroy(gameObject);
    }
}
