using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour // Общий скрипт для всех врагов
{
    [SerializeField] private EnemyParameters _parameters;

    private NavMeshAgent _agent; // NavMeshAgent
    private LevelManager _LM;

    private string _name;

    private int _dropCoins;
    private GameObject _deathEffect;

    #region HP

    private float _hp;
    private float _maxHP;

    #endregion

    [Header("HealthBar")]
    [SerializeField] SpriteRenderer[] strips = new SpriteRenderer[10];
    [SerializeField] private SpriteRenderer healthIcon;
    [SerializeField] Sprite[] healthIcons = new Sprite[4];
    [SerializeField] Sprite[] stripIcons = new Sprite[6];

    #region Protection

    private float _protection;
    private int _shield;
    private const int MAXSHIELD = 10;

    private Tower.DamageTypes _protectionType;

    #endregion

    #region Type

    private EnemyTypes _enemyType;
    public enum EnemyTypes
    {
        None, Boss
    }

    #endregion

    #region Spell

    private EnemySpell _enemySpell;
    public enum EnemySpell
    {
        None, Heal, CreateShield, SpeedBoost, SpawnEnemy
    }

    private float _spellDistance;
    private GameObject _enemySpawn;
    private float _modifier;
    private float _cooldown;

    #endregion

    #region Move

    private float _speed;

    private GameObject[] _points;
    private int _numberPoint = 0;

    private Vector2 _nextPoint;

    private float _distanceToPoint;

    #endregion

    #region Effects

    private bool _immunity;

    private int _poison = 0;

    private bool _ice = false;

    private float _speedBoost;
    private Enemy _speedBooster;

    #endregion

    private void Start()
    {
        #region Parameters

        _name = _parameters.Name;

        _maxHP = _parameters.MaxHP;
        _protection = _parameters.Protection;
        _shield = _parameters.Shield;
        _speed = _parameters.Speed;
        _dropCoins = _parameters.DropCoins;

        _enemyType = _parameters.EnemyType; 
        _protectionType = _parameters.ProtectionType;
        _enemySpell = _parameters.EnemySpell;

        _cooldown = _parameters.SpellColldown;
        _spellDistance = _parameters.SpellDistance;
        _modifier = _parameters.SpellModifier;

        _immunity = _parameters.Immunity;

        #endregion

        #region Resources

        _deathEffect = Resources.Load<GameObject>("Prefabs/Projectiles/DeafEffect");

        #endregion

        _LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        _maxHP = _maxHP * PlayerPrefs.GetFloat("Difficulty");
        _hp = _maxHP;

        _nextPoint = new Vector2(_points[_numberPoint].transform.position.x, _points[_numberPoint].transform.position.z);

        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(new Vector3(_nextPoint.x, 0, _nextPoint.y));

        HealthBar();

        if (_enemySpell != EnemySpell.None)
        {
            StartCoroutine(Spell());
        }
    }

    private void Update()
    {
        #region Distance

        _distanceToPoint = Vector2.Distance(_nextPoint, 
            new Vector2(gameObject.transform.position.x, gameObject.transform.position.z));
        
        if (_distanceToPoint < 0.1f)
        {
            _numberPoint += 1;
            if (_numberPoint >= _points.Length)
            {
                Finish();
            }
            else
            {
                _nextPoint = new Vector2(_points[_numberPoint].transform.position.x, _points[_numberPoint].transform.position.z);
                _agent.SetDestination(new Vector3(_nextPoint.x, 0, _nextPoint.y));
            }
        }

        #endregion

        #region Speed

        if (_speedBooster != null)
        {
            if (Vector2.Distance(new Vector2(_speedBooster.transform.position.x, _speedBooster.transform.position.z)
                , new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) > _speedBooster._spellDistance)
            {
                _speedBoost = 0;
            }
        }

        _agent.speed = _speed + _speedBoost;

        #endregion

        if (_hp <= 0)
        {
            Dead();
        }
    }

    #region GetFunction

    public string GetName()
    {
        return _parameters.Name;
    }

    public Enemy.EnemyTypes GetEnemyType()
    {
        return _enemyType;
    }

    public int GetNumberPoint()
    {
        return _numberPoint;
    }

    public float GetDistanceToPoint()
    {
        return _distanceToPoint;
    }

    public int GetPoison()
    {
        return _poison;
    }

    #endregion

    #region SetFunction

    public void SetPoints(GameObject[] points)
    {
        _points = points;
    }

    #endregion

    #region WaveFunctions

    public void Finish()
    {
        _LM.ReduceHP(_hp);
        if (_enemyType == EnemyTypes.Boss)
        {
            _LM.ReduceHP(10000);
        }
        Destroy(gameObject);
    }

    #endregion

    #region HPFunction

    public void ReduceHP(float damage)
    {
        if (_shield > 0)
        {
            if (damage > 0)
            {
                _shield -= 1;
            }
        }
        else
        {
            _hp -= damage;
            _shield = 0;
        }
        HealthBar();
    }
    public void ReduceHP(float damage, Tower.DamageTypes damageType)
    {
        float trueDamage;

        if (damageType == _protectionType)
        {
            trueDamage = damage - _protection;
            if (trueDamage < 0)
            {
                trueDamage = 0;
            }
        }
        else
        {
            trueDamage = damage;
        }

        if (_shield > 0)
        {
            if (trueDamage > 0)
            {
                _shield -= 1;
            }
        }
        else
        {
            _hp -= trueDamage;
            _shield = 0;
        }
        HealthBar();
    }
    public void ReduceHP(float damage, Tower.DamageTypes damageType, float breakingProtection)
    {
        float trueDamage;

        if (damageType == _protectionType)
        {
            trueDamage = damage - _protection * (1 - breakingProtection); 
            if (trueDamage < 0)
            {
                trueDamage = 0;
            }
        }
        else
        {
            trueDamage = damage;
        }

        if (_shield > 0)
        {
            if (trueDamage > 0)
            {
                _shield -= 1;
            }
        }
        else
        {
            _hp -= trueDamage;
            _shield = 0;
        }
        HealthBar();
    }

    private void HealthBar()
    {
        int a = 0;
        if (_shield > 0)
        {
            a = 1;
        }
        if (_enemyType == EnemyTypes.Boss)
        {
            healthIcon.sprite = healthIcons[2 + a];
        }
        else
        {
            healthIcon.sprite = healthIcons[0 + a];
        }
        foreach (SpriteRenderer strip in strips)
        {
            if (Array.IndexOf(strips, strip) < Convert.ToInt32((Convert.ToSingle(_hp) / Convert.ToSingle(_maxHP)) * 10f)
                || Array.IndexOf(strips, strip) < _shield)
            {
                strip.gameObject.SetActive(true);
                a = 0;
                if (Array.IndexOf(strips, strip) < _shield)
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

    private void Dead()
    {
        Instantiate(_deathEffect, this.transform.position, Quaternion.identity);
        _LM._coins += _dropCoins;
        Destroy(gameObject);
    }

    #endregion

    bool SpellCd = true;

    private IEnumerator Spell()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (!SpellCd)
            {
                for (float i = _cooldown; i > 0; i -= 0.1f)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                SpellCd = true;
            }
            else
            {
                switch (_enemySpell)
                {
                    case EnemySpell.Heal:
                        Enemy[] enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
                        foreach (Enemy enemy in enemyes)
                        {
                            if (Vector3.Distance(gameObject.transform.position, enemy.transform.position) <= _spellDistance && enemy != this && enemy._hp < enemy._maxHP && SpellCd)
                            {
                                GetComponentInChildren<Animator>().SetTrigger("Spell");
                                SpellCd = false;
                                yield return new WaitForSeconds(0.5f);

                                foreach (Enemy enemy1 in enemyes)
                                {
                                    if (Vector3.Distance(gameObject.transform.position, enemy1.transform.position) <= _spellDistance && enemy1 != this)
                                    {
                                        enemy1.Heal(_modifier);
                                    }
                                }
                                break;
                            }
                        }

                        break;
                    case EnemySpell.CreateShield:
                        enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
                        foreach (Enemy enemy in enemyes)
                        {
                            if (Vector3.Distance(gameObject.transform.position, enemy.transform.position) <= _spellDistance && enemy != this && enemy._shield < Enemy.MAXSHIELD && SpellCd)
                            {
                                GetComponentInChildren<Animator>().SetTrigger("Spell");
                                SpellCd = false;
                                yield return new WaitForSeconds(0.5f);

                                foreach (Enemy enemy1 in enemyes)
                                {
                                    if (Vector3.Distance(gameObject.transform.position, enemy1.transform.position) <= _spellDistance && enemy1 != this)
                                    {
                                        enemy1.CreateShield();
                                    }
                                }
                                break;
                            }
                        }

                        break;
                    case EnemySpell.SpeedBoost:
                        enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
                        foreach (Enemy enemy in enemyes)
                        {
                            if (Vector3.Distance(gameObject.transform.position, enemy.transform.position) <= _spellDistance && enemy != this && enemy._shield < Enemy.MAXSHIELD && SpellCd)
                            {
                                GetComponentInChildren<Animator>().SetTrigger("Spell");
                                SpellCd = false;
                                yield return new WaitForSeconds(0.5f);
                                foreach (Enemy enemy1 in enemyes)
                                {
                                    if (Vector3.Distance(gameObject.transform.position, enemy1.transform.position) <= _spellDistance && enemy1 != this)
                                    {
                                        enemy1.SpeedBoost(_modifier, this);
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    case EnemySpell.SpawnEnemy:
                        yield return new WaitForSeconds(0.2f);
                        GameObject _enemy = Instantiate(_enemySpawn, gameObject.transform.position, gameObject.transform.rotation);
                        _enemy.GetComponent<Enemy>()._points = _points;
                        _enemy.GetComponent<Enemy>()._numberPoint = _numberPoint;
                        SpellCd = false ;
                        break;
                }
            }
            
        }
    }

    #region EffectsFunction

    public void Heal(float health)
    {
        _hp += health * PlayerPrefs.GetFloat("Difficulty");
        if (_hp > _maxHP)
        {
            _hp = _maxHP;
        }
        HealthBar();
    }

    public void CreateShield()
    {
        _shield += 1;
        if (_shield > MAXSHIELD)
        {
            _shield = MAXSHIELD;
        }
        HealthBar();
    }

    public void SpeedBoost(float speed, Enemy e)
    {
        _speedBoost = speed;
        _speedBooster = e;
    }

    IEnumerator _Potion(float potionDamage)
    {
        if (_poison < 5)
        {
            _poison += 1;
            if (_poison > 5)
            {
                _poison = 5;
            }
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(5f);
                if (!_immunity)
                {
                    if (_enemyType == EnemyTypes.Boss)
                    {
                        ReduceHP(potionDamage / 5);
                    }
                    else
                    {
                        ReduceHP(potionDamage);
                    }
                }
            }
            yield return new WaitForSeconds(1f);
            _poison -= 1;
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
            if (!_immunity)
            {
                _ice = true;
                float SlowSpeed = _speed * slow;
                _speed -= SlowSpeed;
                yield return new WaitForSeconds(slow * 10f + 1f);
                _speed += SlowSpeed;
                yield return new WaitForSeconds(0.5f);
                _ice = false;
            }
        }
    }
    public void Ice(float slow)
    {
        StartCoroutine(_Ice(slow));
    }

    public void Curse(float damage, float reduceProtection)
    {
        _hp -= damage;
        Curse(reduceProtection);
        HealthBar();
    }
    private void Curse(float curse)
    {
        if (!_immunity)
        {
            _protection -= curse;
            if (_protection < 0)
            {
                _protection = 0;
            }
            if (_shield > 0)
            {
                _shield -= 2;
                if (_shield < 0)
                {
                    _shield = 0;
                }
            }
            HealthBar();
        }
    }

    #endregion 
}
