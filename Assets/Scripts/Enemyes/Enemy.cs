using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour // Общий скрипт для всех врагов
{
    private NavMeshAgent agent; // NavMeshAgent
    private LevelManager LM;

    [SerializeField] int dropCoins;

    [Header("HP")]
    [SerializeField] int hp;
    public int maxHP;

    [Header("Protection")]
    public int protection;
    public int shild;
    public Tower.DamageTypes protectionType;

    [Header("Type")]
    public EnemyTypes enemyType;
    public enum EnemyTypes
    {
        None, Boss
    }

    [Header("Move")]
    public float speed;
    public GameObject[] points;
    public int numberPoint = 0;
    private Vector3 point;

    public float distanceToPoint; // дистанция до следующей точки

    // эфекты
    [SerializeField] bool immunity;
    bool potion;

    private void Start()
    {
        LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        maxHP = hp;
        point = points[numberPoint].transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.destination = point;
        agent.SetDestination(point);
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
                agent.SetDestination(point);
            }
        }
        // NawMesh
        agent.speed = speed;
       

        if (hp <= 0)
        {
            Dead();
        }
    }

    private void Finish() // Действия врага когда он дошёл до конца
    {
        Destroy(gameObject);
    }

    public void ReduceHP(int damage)
    {
        if (shild > 0)
        {
            shild -= 1;
        }
        else
        {
            hp -= damage;
            shild = 0;
        }
    }

    private void Dead() // Действия врага при смерти
    {
        LM.coins += dropCoins;
        Destroy(gameObject);
    }

    public IEnumerator Potion()
    {
        if (!immunity)
        {
            if (!potion)
            {
                potion = true;
                for (float i = 2f; i > 0f; i -= 0.1f)
                {
                    yield return new WaitForSeconds(i);
                    hp -= 1;
                }
                potion = false;
            }
        }
    }
}
