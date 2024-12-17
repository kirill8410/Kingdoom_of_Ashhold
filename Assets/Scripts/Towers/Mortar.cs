using UnityEngine;
using System.Collections;
using NUnit.Framework.Constraints;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem.Switch;

public class Mortar : Tower, TowerFunctions
{
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

    private int trueDamage;

    [SerializeField] GameObject turet;

    public Enemy target;

    private void Start()
    {
        StartCoroutine(SearchTarget());
        StartCoroutine(Attack());
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
            yield return new WaitForSeconds(1f / attackSpeed);
            if ((target != null) && (isAttack) && Vector3.Distance(gameObject.transform.position,
                target.gameObject.transform.position) <= attackDistance)
            {
                trueDamage = damage;
                if (damageType == target.protectionType)
                {
                    trueDamage -= target.protection;
                }
                if (trueDamage < 0)
                {
                    trueDamage = 0;
                }

                switch (mortarType)
                {
                    case MortarType.Simple:
                        RotationTuret();
                        GameObject attack = Instantiate(attackPrefab, new Vector3(target.transform.position.x, 
                            -0.3f, target.transform.position.z), target.transform.rotation);
                        attack.GetComponent<Bomb>().damage = damage;
                        attack.GetComponent<Bomb>().bangDistance = bangDistance;
                        break;
                    case MortarType.Shrapnel:
                        RotationTuret();
                        for (int i = 0; i < 5; i++)
                        {
                            float r = Random.Range(-5, 5);
                            float r1 = Random.Range(-5, 5);
                            attack = Instantiate(attackPrefab, new Vector3(target.transform.position.x + r,
                                -0.3f, target.transform.position.z + r1), target.transform.rotation);
                            attack.GetComponent<Bomb>().damage = damage;
                            attack.GetComponent<Bomb>().bangDistance = bangDistance;
                        }
                        break;
                    case MortarType.Roket:
                        attack = Instantiate(attackPrefab, attackPoint);
                        attack.GetComponent<Roket>().damage = damage;
                        attack.GetComponent<Roket>().bangDistance = bangDistance;
                        attack.GetComponent<Roket>().target = target;
                        break;
                    case MortarType.Fire:
                        RotationTuret();
                        attack = Instantiate(attackPrefab, new Vector3(target.transform.position.x,
                            -0.3f, target.transform.position.z), target.transform.rotation);
                        attack.GetComponent<FireBomb>().damage = damage;
                        attack.GetComponent<FireBomb>().bangDistance = bangDistance;
                        break;
                }
            }
        }
    }
    public IEnumerator SearchTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (target != null && Vector3.Distance(target.transform.position, gameObject.transform.position) > attackDistance)
            {
                target = null;
            }
            Enemy[] enemyes = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (Enemy enemy in enemyes)
            {
                if (Vector3.Distance(gameObject.transform.position, enemy.transform.position) <= attackDistance)
                {
                    if ((target == null) || (enemy.numberPoint > target.numberPoint) ||
                        ((enemy.distanceToPoint < target.distanceToPoint) && (enemy.numberPoint >= target.numberPoint)))
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
            Towerlevel = 2;
        }
        else if (Towerlevel == 2)
        {
            damage += levelUp.damage_2;
            attackDistance += levelUp.distance_2;
            Towerlevel = 3;
        }
    }

    private void RotationTuret()
    {
        turet.transform.LookAt(target.gameObject.transform.position);
    }
}
