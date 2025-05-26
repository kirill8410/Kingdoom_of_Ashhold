using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mortar : Tower, TowerFunctions
{
    public bool isAttack { get; set; } = true;
    public GameObject gm { get; set; }

    [Header("Level")]

    public int PriceLevelUp { get; set; }
    public int Towerlevel { get; set; } = 1;


    [Header("MortarType")]

    [SerializeField] MortarType mortarType;
    private enum MortarType
    {
        Simple, Roket, Fire, Shrapnel
    }
    public float bangDistance;

    [SerializeField] GameObject turet;

    public Enemy target;

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

    public TowerParameters GetParameters()
    {
        return _parameters;
    }

    public IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if ((target != null) && (isAttack) && Vector2.Distance(new Vector2(target.transform.position.x, target.transform.position.z),
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) <= _attackDistance)
            {
                bool _isCooldown = true;
                switch (mortarType)
                {
                    case MortarType.Simple:
                        RotationTuret();
                        GetComponent<Animator>().SetTrigger("Attack");
                        yield return new WaitForSeconds(5f / 60f);
                        if (target != null)
                        {
                            bangDistance = 1.2f + 0.4f * (Towerlevel - 1);
                            GameObject attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Mortars/bom_prefab"), 
                                new Vector3(target.transform.position.x,
                                -0.3f, target.transform.position.z), target.transform.rotation);

                            attack.GetComponent<Bomb>().damage = _damage;
                            attack.GetComponent<Bomb>().bangDistance = bangDistance;
                            attack.transform.localScale = new Vector3(bangDistance, bangDistance, bangDistance);
                        }
                        else
                        {
                            _isCooldown = false;
                        }
                            break;
                    case MortarType.Shrapnel:
                        RotationTuret();
                        GetComponent<Animator>().SetTrigger("Attack");
                        yield return new WaitForSeconds(15f / 60f);
                        if (target != null)
                        {
                            List<Enemy> targets = new List<Enemy>();
                            Enemy[] enemyes = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
                            foreach (Enemy enemy in enemyes)
                            {
                                if (Vector2.Distance(new Vector2(enemy.transform.position.x, enemy.transform.position.z),
                                new Vector2(target.transform.position.x, target.transform.position.z)) <= 2f)
                                {
                                    targets.Add(enemy);
                                }
                            }
                            int numberOfShots = 2;
                            yield return new WaitForSeconds(0.1f);
                            GameObject attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Mortars/bom_prefab"), 
                                new Vector3(target.transform.position.x,
                                    -0.3f, target.transform.position.z), target.transform.rotation);
                            attack.GetComponent<Bomb>().damage = _damage;
                            attack.GetComponent<Bomb>().bangDistance = bangDistance;
                            attack.transform.localScale = new Vector3(bangDistance, bangDistance, bangDistance);
                            if (targets.Count > 1)
                            {
                                for (int i = 0; i < numberOfShots; i++)
                                {
                                    yield return new WaitForSeconds(0.1f);
                                    int random = Random.Range(0, targets.Count);
                                    attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Mortars/bom_prefab"), 
                                        new Vector3(targets[random].transform.position.x,
                                        -0.3f, targets[random].transform.position.z), targets[random].transform.rotation);
                                    attack.GetComponent<Bomb>().damage = _damage;
                                    attack.GetComponent<Bomb>().bangDistance = bangDistance;
                                    attack.transform.localScale = new Vector3(bangDistance, bangDistance, bangDistance);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < numberOfShots; i++)
                                {
                                    yield return new WaitForSeconds(0.1f);
                                    float random1 = Random.Range(-3, 3);
                                    float random2 = Random.Range(-3, 3);
                                    attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Mortars/bom_prefab"), 
                                        new Vector3(target.transform.position.x + random1,
                                        -0.3f, target.transform.position.z + random2), target.transform.rotation);
                                    attack.GetComponent<Bomb>().damage = _damage;
                                    attack.GetComponent<Bomb>().bangDistance = bangDistance;
                                    attack.transform.localScale = new Vector3(bangDistance, bangDistance, bangDistance);
                                }
                            }
                        }
                        else
                        {
                            _isCooldown = false;
                        }
                        break;
                    case MortarType.Roket:
                        GetComponent<Animator>().SetTrigger("Attack");
                        yield return new WaitForSeconds(30f / 60f);
                        if (target != null)
                        {
                            GameObject attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Mortars/roket"), 
                                new Vector3(target.transform.position.x,
                            -0.3f, target.transform.position.z), target.transform.rotation);
                            attack.GetComponent<Bomb>().damage = _damage;
                            attack.GetComponent<Bomb>().bangDistance = bangDistance;
                            attack.transform.localScale = new Vector3(bangDistance, bangDistance, bangDistance);
                            attack.SetActive(true);
                        }
                        else
                        {
                            _isCooldown = false;
                        }
                        break;
                    case MortarType.Fire:
                        RotationTuret();
                        GetComponentInChildren<Animator>().SetTrigger("Attack");
                        yield return new WaitForSeconds(5f / 60f);
                        if (target != null)
                        {
                            GameObject attack = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Mortars/fire_bom_prefab"), 
                                new Vector3(target.transform.position.x,
                            -0.3f, target.transform.position.z), target.transform.rotation);
                            attack.GetComponent<FireBomb>().damage = _damage;
                            attack.GetComponent<FireBomb>().bangDistance = bangDistance;
                            attack.GetComponent<FireBomb>().fireSeconds = 4 + Towerlevel * 2;
                            attack.transform.localScale = new Vector3(bangDistance, bangDistance, bangDistance);
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
                    if ((target == null) || (enemy.GetNumberPoint() > target.GetNumberPoint()) ||
                        ((enemy.GetDistanceToPoint() < target.GetDistanceToPoint()) && (enemy.GetNumberPoint() >= target.GetNumberPoint())))
                    {
                        target = enemy;
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
        turet.transform.LookAt(target.gameObject.transform.position);
    }
}
