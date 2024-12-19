using UnityEngine;
using System.Collections;
using NUnit.Framework.Constraints;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem.Switch;
using System;

public class Mage : Tower, TowerFunctions
{
    public bool isAttack {  get; set; } = true;
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

    [SerializeField] GameObject MageCrystal;

    [Header("MageType")]

    [SerializeField] MageType mageType;
    private enum MageType
    {
        Simple, Fire, Ice, Death, God
    }

    [SerializeField] private int charge = 0;
    private Enemy target2;

    private int trueDamage;

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

                switch (mageType)
                {
                    case MageType.Simple:
                        GameObject attack = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
                        attack.GetComponent<Spell>().damage = trueDamage;
                        attack.GetComponent<Spell>().target = target;
                        attack.GetComponent<Spell>().mage = this;
                        break;
                    case MageType.God:
                        MageCrystal.SetActive(true);
                        attack = Instantiate(attackPrefab, new Vector3(target.transform.position.x, target.transform.position.y + 3f, target.transform.position.z), target.transform.rotation);
                        yield return new WaitForSeconds(0.5f);
                        target.ReduceHP(trueDamage);
                        MageCrystal.SetActive(false);
                        Destroy(attack);
                        break;
                    case MageType.Ice:
                        attack = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
                        attack.GetComponent<IceSpell>().damage = trueDamage;
                        attack.GetComponent<IceSpell>().target = target;
                        attack.GetComponent<IceSpell>().mage = this;
                        break;
                    case MageType.Fire:
                        MageCrystal.SetActive(true);
                        if (target != target2)
                        {
                            charge = 0;
                        }
                        attack = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
                        attack.GetComponent<FireSpell>().target = target;
                        attack.GetComponent<FireSpell>().mage = this;
                        Destroy(attack, (1f / attackSpeed) + 0.3f);
                        trueDamage += damage * charge;
                        charge += 1;
                        target2 = target;
                        target.ReduceHP(trueDamage);
                        break;
                    case MageType.Death:
                        MageCrystal.SetActive(true);
                        isAttack = false;
                        attack = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
                        attack.GetComponent<DeathSpell>().damage = trueDamage;
                        attack.GetComponent<DeathSpell>().target = target;
                        attack.GetComponent<DeathSpell>().mage = this;
                        break;
                }
            }
            else if (mageType == MageType.Fire)
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
            if (target != null && Vector3.Distance(target.transform.position, gameObject.transform.position) > attackDistance)
            {
                target = null;
            }
            Enemy[] enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
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
    public void MageCrystalRecharge(bool b)
    {
        MageCrystal.SetActive(b);
        isAttack = true;
    }
}
