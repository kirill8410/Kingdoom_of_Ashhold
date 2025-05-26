using UnityEngine;
using System.Collections;
using NUnit.Framework.Constraints;
using static UnityEngine.GraphicsBuffer;

public class Ballista : Tower, TowerFunctions // Баллиста
{
    public bool isAttack { get; set; } = true;
    public GameObject gm { get; set; }

    [Header("Level")]

    public int PriceLevelUp { get; set; }
    public int Towerlevel { get; set; } = 1;

    [Header("Turets")]
    [SerializeField] GameObject Turet_osnov;
    [SerializeField] GameObject Turet_osnov2;

    [Header("TowerType")]
    [SerializeField] bool isSniper;
    [SerializeField] bool isDouble;
    [SerializeField] bool isPoison;

    private GameObject _arrow1;
    private GameObject _arrow2;

    public Enemy target;

    private void Start()
    {
        StartCoroutine(SearchTarget());
        StartCoroutine(Attack());
        gm = gameObject;
    }
    private void Update()
    {
        if (target != null)
        {
            RotationTuret();
        }
        _distancePrefab.GetComponent<ParticleSystem>().emissionRate = _attackDistance * 3;
        var shape = _distancePrefab.GetComponent<ParticleSystem>().shape;
        shape.radius = _attackDistance;

    }

    public TowerParameters GetParameters()
    {
        return _parameters;
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
            if ((target != null) && (isAttack) && Vector2.Distance(new Vector2(target.transform.position.x, target.transform.position.z),
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) <= _attackDistance)
            {
                GetComponent<Animator>().SetTrigger("Attack");

                GameObject attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Ballists/Arrow"), _attackPoint.position, _attackPoint.rotation);
                attack.GetComponent<Arrow>().damage = _damage;
                attack.GetComponent<Arrow>().target = target;
                attack.GetComponent<Arrow>().tower = this;
                _arrow1 = attack;

                if (isDouble)
                {
                    GetComponent<Animator>().speed = _attackSpeed;
                    GameObject attack1 = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Ballists/Arrow"), _attackPoint.position, _attackPoint.rotation);
                    attack1.GetComponent<Arrow>().damage = _damage;
                    attack1.GetComponent<Arrow>().target = target;
                    attack1.GetComponent<Arrow>().tower = this;
                    _arrow2 = attack1;
                }
                if (isSniper)
                {
                    attack.GetComponent<Arrow>().towerTransform = gameObject.transform;
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
            if (target != null && Vector2.Distance(new Vector2(target.transform.position.x, target.transform.position.z),
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) > _attackDistance)
            {
                target = null;
            }
            Enemy[] enemyes = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (Enemy enemy in enemyes)
            {
                if (Vector2.Distance(new Vector2(enemy.transform.position.x, enemy.transform.position.z),
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) <= _attackDistance)
                {
                    if (isPoison)
                    {
                        if ((target == null) || (enemy.GetNumberPoint() > target.GetNumberPoint()) || (enemy.GetPoison() < target.GetPoison()))
                        {
                            target = enemy;
                        }
                    }
                    else if (isSniper)
                    {
                        if ((target == null) || (enemy.GetNumberPoint() > target.GetNumberPoint()) ||
                        (Vector2.Distance(new Vector2(enemy.transform.position.x, enemy.transform.position.z),
                    new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) >
                    Vector2.Distance(new Vector2(target.transform.position.x, target.transform.position.z),
                    new Vector2(gameObject.transform.position.x, gameObject.transform.position.z))))
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
        
    }

    private void RotationTuret()
    {
        Turet_osnov.transform.LookAt(target.gameObject.transform.position);
        Turet_osnov2.transform.LookAt(new Vector3(target.gameObject.transform.position.x, Turet_osnov2.transform.position.y, target.gameObject.transform.position.z));
    }
}
