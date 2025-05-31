using UnityEngine;
using System.Collections;
using NUnit.Framework.Constraints;
using static UnityEngine.GraphicsBuffer;
using System.Security.Cryptography;
using Unity.XR.CoreUtils;

public class Ballista : Tower, TowerFunctions // Баллиста
{
    public TowerParameters Parameters { get; set; }

    public bool isAttack { get; set; } = true;
    public GameObject gm { get; set; }

    public int TowerLevel { get; set; } = 1;

    [Header("Turets")]
    [SerializeField] GameObject Turet_osnov;
    [SerializeField] GameObject Turet_osnov2;

    [Header("TowerType")]

    private GameObject _arrow1;
    private GameObject _arrow2;

    private Enemy _target;

    private void Start()
    {
        Parameters = _parameters;
        StartCoroutine(SearchTarget());
        StartCoroutine(Attack());
        gm = gameObject;

        _distancePrefab.GetComponent<ParticleSystem>().emissionRate = _attackDistance * 3;
        var shape = _distancePrefab.GetComponent<ParticleSystem>().shape;
        shape.radius = _attackDistance;
    }

    private void Update()
    {
        if (_target != null)
        {
            RotationTuret();
        }
    }

    public void ArrowSpawn(int arrow)
    {
        
        if (arrow == 1)
        {
            _arrow1.SetActive(true);
        }
        else if (arrow == 2)
        {
            _arrow2.SetActive(true);
        }
    }

    public IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if ((_target != null) && (isAttack) && Vector2.Distance(new Vector2(_target.transform.position.x, _target.transform.position.z),
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) <= _attackDistance)
            {
                GetComponent<Animator>().SetTrigger("Attack");

                GameObject attack;

                switch (_towerType)
                {
                    case TowerTypes.SimpleBallist:
                        attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Ballists/Arrow"), _attackPoint.position, _attackPoint.rotation);
                        attack.GetComponent<Arrow>().Ballista = this;
                        attack.GetComponent<Arrow>().Target = _target;
                        _arrow1 = attack;
                        break;
                    case TowerTypes.DoubleBallist:
                        attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Ballists/Arrow"), _attackPoint.position, _attackPoint.rotation);
                        attack.GetComponent<Arrow>().Ballista = this;
                        attack.GetComponent<Arrow>().Target = _target;
                        _arrow1 = attack;

                        GetComponent<Animator>().speed = _attackSpeed;
                        GameObject attack1 = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Ballists/Arrow"), _attackPoint.position, _attackPoint.rotation);
                        attack.GetComponent<Arrow>().Ballista = this;
                        attack.GetComponent<Arrow>().Target = _target;
                        _arrow2 = attack1;
                        break;
                    case TowerTypes.BigBallist:
                        attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Ballists/Big_arrow"), _attackPoint.position, _attackPoint.rotation);
                        attack.GetComponent<Arrow>().Ballista = this;
                        attack.GetComponent<Arrow>().Target = _target;
                        _arrow1 = attack;
                        break;
                    case TowerTypes.SniperBallist:
                        attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Ballists/Arrow1"), _attackPoint.position, _attackPoint.rotation);
                        attack.GetComponent<Arrow>().Ballista = this;
                        attack.GetComponent<Arrow>().Target = _target;
                        _arrow1 = attack;
                        break;
                    case TowerTypes.PoisonBallist:
                        attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Ballists/Poison_arrow"), _attackPoint.position, _attackPoint.rotation);
                        attack.GetComponent<Arrow>().Ballista = this;
                        attack.GetComponent<Arrow>().Target = _target;
                        _arrow1 = attack;
                        break;
                }
                for (float i = 1 / _attackSpeed; i > 0; i -= 0.1f)
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    public IEnumerator SearchTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (_target != null && Vector2.Distance(new Vector2(_target.transform.position.x, _target.transform.position.z),
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) > _attackDistance)
            {
                _target = null;
            }
            Enemy[] enemyes = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (Enemy enemy in enemyes)
            {
                if (Vector2.Distance(new Vector2(enemy.transform.position.x, enemy.transform.position.z),
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) <= _attackDistance)
                {
                    if (_towerType == TowerTypes.PoisonBallist)
                    {
                        if ((_target == null) || (enemy.GetNumberPoint() > _target.GetNumberPoint()) || (enemy.GetPoison() < _target.GetPoison()))
                        {
                            _target = enemy;
                        }
                    }
                    else if (_towerType == TowerTypes.SniperBallist)
                    {
                        if ((_target == null) || (enemy.GetNumberPoint() > _target.GetNumberPoint()) ||
                        (Vector2.Distance(new Vector2(enemy.transform.position.x, enemy.transform.position.z),
                    new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) >
                    Vector2.Distance(new Vector2(_target.transform.position.x, _target.transform.position.z),
                    new Vector2(gameObject.transform.position.x, gameObject.transform.position.z))))
                        {
                            _target = enemy;
                        }
                    }
                    else
                    {
                        if ((_target == null) || (enemy.GetNumberPoint() > _target.GetNumberPoint()) ||
                        ((enemy.GetDistanceToPoint() < _target.GetDistanceToPoint()) && (enemy.GetNumberPoint() >= _target.GetNumberPoint())))
                        {
                            _target = enemy;
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
            _breakingProtection = Parameters.BreakingProtection_2;
            TowerLevel = 2;
        }
        else if (TowerLevel == 2)
        {
            _damage = Parameters.Damage_3;
            _attackDistance = Parameters.AttackDistance_3;
            _attackSpeed = Parameters.AttackSpeed_3;
            _breakingProtection = Parameters.BreakingProtection_3;
            TowerLevel = 3;
        }
    }

    private void RotationTuret()
    {
        Turet_osnov.transform.LookAt(_target.gameObject.transform.position);
        Turet_osnov2.transform.LookAt(new Vector3(_target.gameObject.transform.position.x, Turet_osnov2.transform.position.y, _target.gameObject.transform.position.z));
    }
}
