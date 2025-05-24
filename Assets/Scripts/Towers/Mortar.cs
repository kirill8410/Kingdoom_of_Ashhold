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

    [SerializeField] LevelUp _levelUp;
    public LevelUp levelUp
    {
        get
        {
            return _levelUp;
        }
        set
        {
            _levelUp = value;
        }
    }

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
        Distance.transform.localScale = new Vector3(attackDistance * 2f, attackDistance * 2f, 1f);
        if (Towerlevel == 1)
        {
            PriceLevelUp = levelUp.priceLevelUp_1;
        }
        else if (Towerlevel == 2)
        {
            PriceLevelUp = levelUp.priceLevelUp_2;
        }
    }

    public IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if ((target != null) && (isAttack) && Vector2.Distance(new Vector2(target.transform.position.x, target.transform.position.z),
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) <= attackDistance)
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
                            GameObject attack = Instantiate(attackPrefab, new Vector3(target.transform.position.x,
                                -0.3f, target.transform.position.z), target.transform.rotation);

                            attack.GetComponent<Bomb>().damage = damage;
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
                            GameObject attack = Instantiate(attackPrefab, new Vector3(target.transform.position.x,
                                    -0.3f, target.transform.position.z), target.transform.rotation);
                            attack.GetComponent<Bomb>().damage = damage;
                            attack.GetComponent<Bomb>().bangDistance = bangDistance;
                            attack.transform.localScale = new Vector3(bangDistance, bangDistance, bangDistance);
                            if (targets.Count > 1)
                            {
                                for (int i = 0; i < numberOfShots; i++)
                                {
                                    yield return new WaitForSeconds(0.1f);
                                    int random = Random.Range(0, targets.Count);
                                    attack = Instantiate(attackPrefab, new Vector3(targets[random].transform.position.x,
                                        -0.3f, targets[random].transform.position.z), targets[random].transform.rotation);
                                    attack.GetComponent<Bomb>().damage = damage;
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
                                    attack = Instantiate(attackPrefab, new Vector3(target.transform.position.x + random1,
                                        -0.3f, target.transform.position.z + random2), target.transform.rotation);
                                    attack.GetComponent<Bomb>().damage = damage;
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
                            GameObject attack = Instantiate(attackPrefab, new Vector3(target.transform.position.x,
                            -0.3f, target.transform.position.z), target.transform.rotation);
                            attack.GetComponent<Bomb>().damage = damage;
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
                            GameObject attack = Instantiate(attackPrefab, new Vector3(target.transform.position.x,
                            -0.3f, target.transform.position.z), target.transform.rotation);
                            attack.GetComponent<FireBomb>().damage = damage;
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
                    for (float i = 1 / attackSpeed; i > 0; i -= 0.1f)
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
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) > attackDistance)
                
            {
                target = null;
            }
            Enemy[] enemyes = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (Enemy enemy in enemyes)
            {
                if (Vector2.Distance(new Vector2(enemy.transform.position.x, enemy.transform.position.z),
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) <= attackDistance)
                {
                    if ((target == null) || (enemy._numberPoint > target._numberPoint) ||
                        ((enemy._distanceToPoint < target._distanceToPoint) && (enemy._numberPoint >= target._numberPoint)))
                    {
                        target = enemy;
                    }
                }
            }
        }
    }
    public void LevelUp()
    {
        if (Towerlevel == 1)
        {
            damage += levelUp.damage_1;
            attackDistance += levelUp.distance_1;
            attackSpeed += levelUp.attackSpeed_1;
            Towerlevel = 2;
        }
        else if (Towerlevel == 2)
        {
            damage += levelUp.damage_2;
            attackDistance += levelUp.distance_2;
            attackSpeed += levelUp.attackSpeed_2;
            Towerlevel = 3;
        }
    }

    private void RotationTuret()
    {
        turet.transform.LookAt(target.gameObject.transform.position);
    }
}
