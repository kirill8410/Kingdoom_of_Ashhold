using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.XR.CoreUtils;

public class Mortar : Tower, TowerFunctions
{
    public TowerParameters Parameters { get; set; }

    public bool isAttack { get; set; } = true;
    public GameObject gm { get; set; }

    [Header("Level")]

    public int PriceLevelUp { get; set; }
    public int TowerLevel { get; set; } = 1;

    [SerializeField] GameObject turet;

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

    public IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if ((_target != null) && (isAttack) && Vector2.Distance(new Vector2(_target.transform.position.x, _target.transform.position.z),
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) <= _attackDistance)
            {
                bool _isCooldown = true;
                switch (_towerType)
                {
                    case TowerTypes.SimpleMortar:
                        RotationTuret();
                        GetComponent<Animator>().SetTrigger("Attack");
                        yield return new WaitForSeconds(5f / 60f);
                        if (_target != null)
                        {
                            GameObject attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Mortars/bom_prefab"), 
                                new Vector3(_target.transform.position.x,
                                -0.3f, _target.transform.position.z), _target.transform.rotation);

                            attack.GetComponent<Bomb>().Mortar = this;
                            attack.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                        }
                        else
                        {
                            _isCooldown = false;
                        }
                            break;
                    case TowerTypes.ShrapnelMortar:
                        RotationTuret();
                        GetComponent<Animator>().SetTrigger("Attack");
                        yield return new WaitForSeconds(15f / 60f);
                        if (_target != null)
                        {
                            List<Enemy> targets = new List<Enemy>();
                            Enemy[] enemyes = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
                            foreach (Enemy enemy in enemyes)
                            {
                                if (Vector2.Distance(new Vector2(enemy.transform.position.x, enemy.transform.position.z),
                                new Vector2(_target.transform.position.x, _target.transform.position.z)) <= 4f)
                                {
                                    targets.Add(enemy);
                                }
                            }
                            int numberOfShots = TowerLevel + 1;
                            yield return new WaitForSeconds(0.1f);
                            GameObject attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Mortars/bom_prefab"), 
                                new Vector3(_target.transform.position.x,
                                    -0.3f, _target.transform.position.z), _target.transform.rotation);
                            attack.GetComponent<Bomb>().Mortar = this;
                            attack.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                            if (targets.Count > 1)
                            {
                                for (int i = 0; i < numberOfShots; i++)
                                {
                                    yield return new WaitForSeconds(0.1f);
                                    int random = Random.Range(0, targets.Count - 1);
                                    attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Mortars/bom_prefab"), 
                                        new Vector3(targets[random].transform.position.x,
                                        -0.3f, targets[random].transform.position.z), targets[random].transform.rotation);
                                    attack.GetComponent<Bomb>().Mortar = this;
                                    attack.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                                }
                            }
                        }
                        else
                        {
                            _isCooldown = false;
                        }
                        break;
                    case TowerTypes.RoketMortar:
                        GetComponent<Animator>().SetTrigger("Attack");
                        yield return new WaitForSeconds(30f / 60f);
                        if (_target != null)
                        {
                            GameObject attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Mortars/roket"), 
                                new Vector3(_target.transform.position.x,
                            -0.3f, _target.transform.position.z), _target.transform.rotation);
                            attack.GetComponent<Bomb>().Mortar = this;
                            attack.transform.localScale = new Vector3(2.4f, 2.4f, 2.4f);
                            attack.SetActive(true);
                        }
                        else
                        {
                            _isCooldown = false;
                        }
                        break;
                    case TowerTypes.FireMortar:
                        RotationTuret();
                        GetComponentInChildren<Animator>().SetTrigger("Attack");
                        yield return new WaitForSeconds(5f / 60f);
                        if (_target != null)
                        {
                            GameObject attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Mortars/fire_bom_prefab"), 
                                new Vector3(_target.transform.position.x,
                            -0.3f, _target.transform.position.z), _target.transform.rotation);
                            attack.GetComponent<FireBomb>().Mortar = this;
                            attack.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                            attack.GetComponent<FireBomb>().fireSeconds = 4 + TowerLevel * 2;
                        }
                        else
                        {
                            _isCooldown = false;
                        }
                        break;
                }
                if (_isCooldown)
                {
                    for (float i = 1 / _attackSpeed; i > 0; i -= 0.1f)
                    {
                        yield return new WaitForSeconds(0.1f);
                    }
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
                    if ((_target == null) || (enemy.GetNumberPoint() > _target.GetNumberPoint()) ||
                        ((enemy.GetDistanceToPoint() < _target.GetDistanceToPoint()) && (enemy.GetNumberPoint() >= _target.GetNumberPoint())))
                    {
                        _target = enemy;
                    }
                }
            }
        }
    }

    public void LevelUp()
    {
        if (TowerLevel == 1)
        {
            _damage = _parameters.Damage_2;
            _attackDistance = _parameters.AttackDistance_2;
            _attackSpeed = _parameters.AttackSpeed_2;
            TowerLevel = 2;
        }
        else if (TowerLevel == 2)
        {
            _damage = _parameters.Damage_3;
            _attackDistance = _parameters.AttackDistance_3;
            _attackSpeed = _parameters.AttackSpeed_3;
            TowerLevel = 3;
        }
    }

    private void RotationTuret()
    {
        turet.transform.LookAt(_target.gameObject.transform.position);
    }
}
