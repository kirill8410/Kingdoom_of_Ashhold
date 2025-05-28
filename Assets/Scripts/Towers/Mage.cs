using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using Unity.XR.CoreUtils;

public class Mage : Tower, TowerFunctions
{
    public TowerParameters Parameters { get; set; }

    public bool isAttack {  get; set; } = true;
    public GameObject gm { get; set; }

    [Header("Level")]
    public int TowerLevel { get; set; } = 1;

    [SerializeField] GameObject MageCrystal;

    [SerializeField] private int charge = 0;

    private Enemy target2;
    public Enemy target;

    private void Awake()
    {
        _distancePrefab = gameObject.GetNamedChild("Distance");

        _damage = Parameters.Damage_1;
        _attackSpeed = Parameters.AttackSpeed_1;
        _attackDistance = Parameters.AttackDistance_1;
        _breakingProtection = Parameters.BreakingProtection_1;

        _damageType = Parameters.DamageType;
        _towerType = Parameters.TowerType;
    }

    private void Start()
    {
        StartCoroutine(SearchTarget());
        StartCoroutine(Attack());
        gm = gameObject;
    }

    private void Update()
    {
        _distancePrefab.GetComponent<ParticleSystem>().emissionRate = _attackDistance * 3;
        var shape = _distancePrefab.GetComponent<ParticleSystem>().shape;
        shape.radius = _attackDistance;

    }



    public IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if ((target != null) && (isAttack) && Vector2.Distance(new Vector2(target.transform.position.x, target.transform.position.z),
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) <= _attackDistance)
            {
                switch (_towerType)
                {
                    case TowerTypes.SimpleMage:
                        GameObject attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Mage/Spell"), 
                            _attackPoint.position, _attackPoint.rotation);
                        attack.GetComponent<Spell>().damage = _damage;
                        attack.GetComponent<Spell>().target = target;
                        attack.GetComponent<Spell>().mage = this;
                        break;
                    case TowerTypes.GodMage:
                        Enemy _target = target;
                        MageCrystal.SetActive(true);
                        yield return new WaitForSeconds(2f);
                        if (target != null && target == _target)
                        {
                            if (target.GetEnemyType() == Enemy.EnemyTypes.Boss)
                            {
                                target.ReduceHP(_damage / 2);
                            }
                            else
                            {
                                target.ReduceHP(_damage);
                            }
                        } 
                        MageCrystal.SetActive(false);
                        break;
                    case TowerTypes.IceMage:
                        attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Mage/IceSpell"), 
                            _attackPoint.position, _attackPoint.rotation);
                        attack.GetComponent<IceSpell>().damage = _damage;
                        attack.GetComponent<IceSpell>().target = target;
                        if (TowerLevel == 3)
                        {
                            attack.GetComponent<IceSpell>().slow = 0.6f;
                        }
                        break;
                    case TowerTypes.FireMage:
                        MageCrystal.SetActive(true);
                        if (target != target2)
                        {
                            charge = 0;
                        }
                        attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Mage/FireSpell"), 
                            _attackPoint.position, _attackPoint.rotation);
                        attack.GetComponent<FireSpell>().target = target;
                        attack.GetComponent<FireSpell>().mage = this;
                        charge += 1;
                        target2 = target;
                        if (target.GetEnemyType() != Enemy.EnemyTypes.Boss)
                        {
                            target.ReduceHP(_damage + charge);
                        }
                        else
                        {
                            target.ReduceHP(charge);
                        }
                        Destroy(attack, 2f);
                        break;
                    case TowerTypes.DeathMage:
                        MageCrystal.SetActive(true);
                        attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Mage/DeathSpell"), 
                            _attackPoint.position, _attackPoint.rotation);
                        attack.GetComponent<DeathSpell>().damage = _damage;
                        attack.GetComponent<DeathSpell>().target = target;
                        attack.GetComponent<DeathSpell>().mage = this;
                        break;
                }
                for (float i = 1 / _attackSpeed; i > 0; i -= 0.1f)
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else if (_towerType == TowerTypes.FireMage)
            {
                MageCrystal.SetActive(false);
                target2 = null;
            }
        }
    }

    public IEnumerator SearchTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (target != null && Vector2.Distance(new Vector2(target.transform.position.x, target.transform.position.z),
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) > _attackDistance)
            {
                target = null;
            }
            Enemy[] enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (Enemy enemy in enemyes)
            {
                if (Vector2.Distance(new Vector2(enemy.transform.position.x, enemy.transform.position.z),
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) <= _attackDistance)
                {
                    if (_towerType == TowerTypes.DeathMage)
                    {
                        if ((target == null) || (enemy.GetNumberPoint() > target.GetNumberPoint()) ||
                        (Vector2.Distance(new Vector2(enemy.transform.position.x, enemy.transform.position.z),
                    new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) <
                    Vector2.Distance(new Vector2(target.transform.position.x, target.transform.position.z),
                    new Vector2(gameObject.transform.position.x, gameObject.transform.position.z))))
                        {
                            target = enemy;
                        }
                    }
                    else if (_towerType == TowerTypes.FireMage)
                    {
                        if (target == null)
                        {
                            target = enemy;
                        }
                    }
                    else
                    {
                        if ((target == null) || (enemy.GetNumberPoint() > target.GetNumberPoint()) ||
                        ((enemy.GetDistanceToPoint() < target.GetDistanceToPoint()) && (enemy.GetNumberPoint() >= target.GetNumberPoint())))
                        {
                            target = enemy;
                        }
                    }
                }
            }
        }
    }

    public void LevelUp()
    {
        if (TowerLevel == 1)
        {
            _damage = Parameters.Damage_2;
            _attackDistance = Parameters.AttackDistance_2;
            _attackSpeed = Parameters.AttackSpeed_2;
            TowerLevel = 2;
        }
        else if (TowerLevel == 2)
        {
            _damage = Parameters.Damage_3;
            _attackDistance = Parameters.AttackDistance_3;
            _attackSpeed = Parameters.AttackSpeed_3;
            TowerLevel = 3;
        }
    }

    public void MageCrystalRecharge(bool b)
    {
        MageCrystal.SetActive(b);
    }
}
