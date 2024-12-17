using System;
using System.Collections;
using TMPro;
using UnityEditor;
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

    [Header("HealthBar")]
    [SerializeField] SpriteRenderer[] strips = new SpriteRenderer[10];
    [SerializeField] SpriteRenderer healthIcon;
    [SerializeField] Sprite[] healthIcons = new Sprite[4];
    [SerializeField] Sprite[] stripIcons = new Sprite[6];

    [Header("Protection")]
    public int protection;
    [SerializeField] int shield;
    private int maxShield = 10;

    public Tower.DamageTypes protectionType;

    [Header("Type")]
    public EnemyTypes enemyType;
    public enum EnemyTypes
    {
        None, Boss
    }

    [Header("Spell")]
    [SerializeField] EnemySpell enemySpell;
    public enum EnemySpell
    {
        None, Heal, CreateShield, SpeedBoost, SpawnEnemy
    }
    [SerializeField] float spellDistance;
    [SerializeField] GameObject enemySpawn;
    [SerializeField] int heal;
    [SerializeField] float speedBoost;
    [SerializeField] float cooldown;


    [Header("Move")]
    public float speed = 1;
    public GameObject[] points;
    public int numberPoint = 0;
    private Vector3 point;

    public float distanceToPoint; // дистанция до следующей точки

    // эфекты
    [SerializeField] bool immunity = false;
    int _potion = 0;
    bool _ice = false;
    private float _speed = 0;
    private Enemy speedBooster;

    private void Start()
    {
        LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        hp = hp * PlayerPrefs.GetInt("Difficulty");
        maxHP = hp;
        point = points[numberPoint].transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.destination = point;
        agent.SetDestination(point);
        HealthBar();
        if (enemySpell != EnemySpell.None)
        {
            StartCoroutine(Spell());
        }
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
        if (speedBooster != null)
        {
            if (Vector3.Distance(speedBooster.transform.position, gameObject.transform.position) > speedBooster.spellDistance)
            {
                _speed = 0;
            }
        }
        agent.speed = speed + _speed;       

        if (hp <= 0)
        {
            Dead();
        }
    }

    private void Finish() // Действия врага когда он дошёл до конца
    {
        LM.HP -= hp;
        if (enemyType == EnemyTypes.Boss)
        {
            LM.HP = 0;
        }
        Destroy(gameObject);
    }

    public void ReduceHP(int damage)
    {
        if (shield > 0)
        {
            shield -= 1;
        }
        else
        {
            hp -= damage;
            shield = 0;
        }
        HealthBar();
    }
    public void Heal(int health)
    {
        hp += health;
        if (hp > maxHP)
        {
            hp = maxHP;
        }
        HealthBar();
    }
    public void CreateShield()
    {
        shield += 1;
        if (shield > maxShield)
        {
            shield = maxShield;
        }
        HealthBar();
    }
    public void SpeedBoost(float speed, Enemy e)
    {
        _speed = speed;
        speedBooster = e;
    }

    private void HealthBar()
    {
        int a = 0;
        if (shield > 0)
        {
            a = 1;
        }
        if (enemyType == EnemyTypes.Boss)
        {
            healthIcon.sprite = healthIcons[2 + a];
        }
        else
        {
            healthIcon.sprite = healthIcons[0 + a];
        }
        foreach (SpriteRenderer strip in strips)
        {
            if (Array.IndexOf(strips, strip) < Convert.ToInt32((Convert.ToSingle(hp)/ Convert.ToSingle(maxHP))*10f) 
                || Array.IndexOf(strips, strip) < shield)
            {
                strip.gameObject.SetActive(true);
                a = 0;
                if (Array.IndexOf(strips, strip) < shield)
                {
                    a = 3;
                }
                if (strip == strips[0])
                {
                    strip.sprite = stripIcons[0 + a];
                }
                else if (strip == strips[strips.Length - 1])
                {
                    strip.sprite = stripIcons[2 + a];
                }
                else
                {
                    strip.sprite = stripIcons[1 + a];
                }
            }
            else
            {
                strip.gameObject.SetActive(false);
            }
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
                    if (enemyType == EnemyTypes.Boss)
                    {
                        hp += potionDamage - 1;
                    }
                    ReduceHP(potionDamage);
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
            if (!immunity)
            {
                _ice = true;
                float SlowSpeed = speed * 0.6f;
                speed -= SlowSpeed;
                yield return new WaitForSeconds(10f);
                speed += SlowSpeed;
                _ice = false;
            }
        }
    }
    public void Ice()
    {
        StartCoroutine(_Ice());
    }

    public void Curse(int curse)
    {
        if (!immunity)
        {
            protection -= curse;
            if (shield > 0)
            {
                shield -= 1;
            }
        }
    }

    private IEnumerator Spell()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown);
            switch (enemySpell)
            {
                case EnemySpell.Heal:
                    float s = speed;
                    speed = 0;
                    GetComponentInChildren<Animator>().SetTrigger("Spell");
                    yield return new WaitForSeconds(1f);
                    Enemy[] enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
                    foreach (Enemy enemy in enemyes)
                    {
                        if (Vector3.Distance(gameObject.transform.position, enemy.transform.position) <= spellDistance && enemy != this)
                        {
                            enemy.Heal(heal);
                        }
                    }
                    speed = s;
                    break;
                case EnemySpell.CreateShield:
                    s = speed;
                    speed = 0;
                    GetComponentInChildren<Animator>().SetTrigger("Spell");
                    yield return new WaitForSeconds(1f);
                    enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
                    foreach (Enemy enemy in enemyes)
                    {
                        if (Vector3.Distance(gameObject.transform.position, enemy.transform.position) <= spellDistance && enemy != this)
                        {
                            enemy.CreateShield();
                        }
                    }
                    speed = s;
                    break;
                case EnemySpell.SpeedBoost:
                    s = speed;
                    speed = 0;
                    GetComponentInChildren<Animator>().SetTrigger("Spell");
                    yield return new WaitForSeconds(1f);
                    enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
                    foreach (Enemy enemy in enemyes)
                    {
                        if (Vector3.Distance(gameObject.transform.position, enemy.transform.position) <= spellDistance && enemy != this)
                        {
                            enemy.SpeedBoost(speedBoost, this);
                        }
                    }
                    speed = s;
                    break;
                case EnemySpell.SpawnEnemy:
                    s = speed;
                    speed = 0;
                    yield return new WaitForSeconds(0.5f);
                    GameObject _enemy = Instantiate(enemySpawn, gameObject.transform.position, gameObject.transform.rotation);
                    _enemy.GetComponent<Enemy>().points = points;
                    _enemy.GetComponent<Enemy>().numberPoint = numberPoint;
                    speed = s;
                    break;
            }
        }
    }
}
