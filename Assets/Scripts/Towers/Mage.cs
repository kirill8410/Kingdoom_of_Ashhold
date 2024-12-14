using UnityEngine;
using System.Collections;
using NUnit.Framework.Constraints;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem.Switch;

public class Mage : Tower, TowerFunctions
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

    [SerializeField] GameObject MageCrystal;

    [Header("MageType")]

    [SerializeField] MageType mageType;
    private enum MageType
    {
        Simple, Fire, Ice, Death, God
    }

    private int charge = 0;
    private Enemy target2;

    private int trueDamage;

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

                switch (mageType)
                {
                    case MageType.Simple:
                        MageCrystal.SetActive(false);
                        isAttack = false;
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
                        MageCrystal.SetActive(false);
                        isAttack = false;
                        attack = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
                        attack.GetComponent<IceSpell>().damage = trueDamage;
                        attack.GetComponent<IceSpell>().target = target;
                        attack.GetComponent<IceSpell>().mage = this;
                        break;
                    case MageType.Fire:
                        MageCrystal.SetActive(true);
                        attack = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
                        attack.GetComponent<FireSpell>().target = target;
                        if (target != target2)
                        {
                            charge = 0;
                        }
                        trueDamage += damage * charge;
                        charge += 1;
                        target2 = target;
                        target.ReduceHP(trueDamage);
                        break;
                }
            }
            else if (mageType == MageType.Fire)
            {
                MageCrystal.SetActive(false);
            }
        }
    }
    public IEnumerator SearchTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
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
    public void MageCrystalRecharge()
    {
        MageCrystal.SetActive(true);
        isAttack = true;
    }
}
