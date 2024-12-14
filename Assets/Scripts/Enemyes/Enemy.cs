using System;
using System.Collections;
using TMPro;
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
    [SerializeField] TextMeshProUGUI HPText;

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

    public enum EnemySpell
    {
        None, Health, CreateShild, SpeedBust, SpawnEnemy
    }
    public EnemySpell enemySpell;

    [Header("Move")]
    public float speed;
    public GameObject[] points;
    public int numberPoint = 0;
    private Vector3 point;

    public float distanceToPoint; // дистанция до следующей точки

    // эфекты
    [SerializeField] bool immunity = false;
    int _potion = 0;
    bool _ice = false;

    private void Start()
    {
        LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        hp = hp * PlayerPrefs.GetInt("Difficulty");
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
        if (distanceToPoint < 0.35f)
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

        // Отображение здоровья
        HPText.text = $"{hp}/{maxHP}"; 
    }

    private void Finish() // Действия врага когда он дошёл до конца
    {
        LM.HP -= 1;
        if (enemyType == EnemyTypes.Boss)
        {
            LM.HP = 0;
        }
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

    IEnumerator _Potion(int potionDamage)
    {
        if (_potion <= 5)
        {
            _potion += 1;
            if (_potion > 5)
            {
                _potion = 5;
            }
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(2f);
                if (!immunity)
                {
                    hp -= potionDamage;
                    if (enemyType == EnemyTypes.Boss)
                    {
                        hp += potionDamage - 1;
                    }
                }
            }
            _potion -= 1;
        }
    }
    public void Potion(int potionDamage)
    {
        StartCoroutine(_Potion(potionDamage));
    }

    private IEnumerator _Ice()
    {
        if (!_ice)
        {
            _ice = true;
            float SlowSpeed = speed * 0.6f;
            speed -= SlowSpeed;
            yield return new WaitForSeconds(10f);
            speed += SlowSpeed;
            _ice = false;
        }
    }

    public void Ice()
    {
        StartCoroutine(_Ice());
    }
}
