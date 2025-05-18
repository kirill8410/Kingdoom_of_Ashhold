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

    public string Name;

    [SerializeField] int dropCoins;

    [Header("HP")]
    [SerializeField] float hp;
    public float maxHP;

    [Header("HealthBar")]
    [SerializeField] SpriteRenderer[] strips = new SpriteRenderer[10];
    [SerializeField] SpriteRenderer healthIcon;
    [SerializeField] Sprite[] healthIcons = new Sprite[4];
    [SerializeField] Sprite[] stripIcons = new Sprite[6];
    [SerializeField] GameObject effect;

    [Header("Protection")]
    public float protection;
    public int shield;
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
    [SerializeField] float heal;
    [SerializeField] float speedBoost;
    [SerializeField] float cooldown;


    [Header("Move")]
    public float speed = 1;
    public GameObject[] points;
    public int numberPoint = 0;
    private Vector2 point;

    public float distanceToPoint; // дистанция до следующей точки

    // эфекты
    [SerializeField] bool immunity = false;
    public int _potion = 0;
    bool _ice = false;
    private float _speed = 0;
    private Enemy speedBooster;

    private void Start()
    {
        LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        hp = hp * PlayerPrefs.GetFloat("Difficulty");
        maxHP = hp;
        point = new Vector2(points[numberPoint].transform.position.x, points[numberPoint].transform.position.z);
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(new Vector3(point.x, 0, point.y));
        HealthBar();
        if (enemySpell != EnemySpell.None)
        {
            StartCoroutine(Spell());
        }
    }

    private void Update()
    {
        // Определение дистанции до точки
        distanceToPoint = Vector2.Distance(point, 
            new Vector2(gameObject.transform.position.x, gameObject.transform.position.z));
        // Переключение на следующую точку
        if (distanceToPoint < 0.1f)
        {
            numberPoint += 1;
            if (numberPoint >= points.Length)
            {
                Finish();
            }
            else
            {
                point = new Vector2(points[numberPoint].transform.position.x, points[numberPoint].transform.position.z);
                agent.SetDestination(new Vector3(point.x, 0, point.y));
            }
        }
        // NawMesh
        if (speedBooster != null)
        {
            if (Vector2.Distance(new Vector2(speedBooster.transform.position.x, speedBooster.transform.position.z)
                , new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) > speedBooster.spellDistance)
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

    public void Finish() // Действия врага когда он дошёл до конца
    {
        LM.ReduceHP(hp);
        if (enemyType == EnemyTypes.Boss)
        {
            LM.ReduceHP(10000);
        }
        Destroy(gameObject);
    }

    public void ReduceHP(float damage)
    {
        if (shield > 0)
        {
            if (damage > 0)
            {
                shield -= 1;
            }
        }
        else
        {
            hp -= damage;
            shield = 0;
        }
        HealthBar();
    }
    public void ReduceHP(float damage, float reduceProtection)
    {
        hp -= damage;
        Curse(reduceProtection);
        HealthBar();
    }
    public void Heal(float health)
    {
        hp += health * PlayerPrefs.GetFloat("Difficulty");
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
        Instantiate(effect, this.transform.position, Quaternion.identity);
        LM._coins += dropCoins;
        Destroy(gameObject);
    }

    IEnumerator _Potion(float potionDamage)
    {
        if (_potion < 5)
        {
            _potion += 1;
            if (_potion > 5)
            {
                _potion = 5;
            }
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(5f);
                if (!immunity)
                {
                    if (enemyType == EnemyTypes.Boss)
                    {
                        hp -= potionDamage / 5;
                    }
                    ReduceHP(potionDamage);
                    print("1");
                }
            }
            yield return new WaitForSeconds(1f);
            _potion -= 1;
            print("2");
        }
    }
    public void Potion(float potionDamage)
    {
        StartCoroutine(_Potion(potionDamage));
    }

    private IEnumerator _Ice(float slow)
    {
        if (!_ice)
        {
            if (!immunity)
            {
                _ice = true;
                float SlowSpeed = speed * slow;
                speed -= SlowSpeed;
                yield return new WaitForSeconds(slow * 10f + 1f);
                speed += SlowSpeed;
                yield return new WaitForSeconds(0.5f);
                _ice = false;
            }
        }
    }
    public void Ice(float slow)
    {
        StartCoroutine(_Ice(slow));
    }

    public void Curse(float curse)
    {
        if (!immunity)
        {
            protection -= curse;
            if (protection < -10)
            {
                protection = -10;
            }
            if (shield > 0)
            {
                shield -= 2;
            }
            HealthBar();
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
                    GetComponentInChildren<Animator>().SetTrigger("Spell");
                    float s = speed;
                    speed = 0;
                    yield return new WaitForSeconds(0.5f);
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
                    GetComponentInChildren<Animator>().SetTrigger("Spell");
                    s = speed;
                    speed = 0;
                    yield return new WaitForSeconds(0.5f);
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
                    GetComponentInChildren<Animator>().SetTrigger("Spell");
                    s = speed;
                    speed = 0;
                    yield return new WaitForSeconds(0.5f);
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
                    yield return new WaitForSeconds(0.2f);
                    GameObject _enemy = Instantiate(enemySpawn, gameObject.transform.position, gameObject.transform.rotation);
                    _enemy.GetComponent<Enemy>().points = points;
                    _enemy.GetComponent<Enemy>().numberPoint = numberPoint;
                    break;
            }
        }
    }
}
