using UnityEngine;
using System.Collections;

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

    private float trueDamage;

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
                switch (mageType)
                {
                    case MageType.Simple:
                        GameObject attack = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
                        attack.GetComponent<Spell>().damage = damage;
                        attack.GetComponent<Spell>().target = target;
                        attack.GetComponent<Spell>().mage = this;
                        break;
                    case MageType.God:
                        Enemy _target = target;
                        trueDamage = damage;
                        if (target.protectionType == DamageTypes.Magic)
                        {
                            trueDamage -= target.protection * 2;
                        }
                        if (trueDamage < 0)
                        {
                            trueDamage = 0;
                        }
                        if (target.enemyType == Enemy.EnemyTypes.Boss)
                        {
                            trueDamage /= 2;
                        }
                        MageCrystal.SetActive(true);
                        yield return new WaitForSeconds(2f);
                        if (target != null && target == _target)
                        {
                            target.ReduceHP(trueDamage);
                        } 
                        MageCrystal.SetActive(false);
                        break;
                    case MageType.Ice:
                        attack = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
                        attack.GetComponent<IceSpell>().damage = damage;
                        attack.GetComponent<IceSpell>().target = target;
                        if (Towerlevel == 3)
                        {
                            attack.GetComponent<IceSpell>().slow = 0.6f;
                        }
                        break;
                    case MageType.Fire:
                        trueDamage = damage;
                        MageCrystal.SetActive(true);
                        if (target != target2)
                        {
                            charge = 0;
                        }
                        attack = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
                        attack.GetComponent<FireSpell>().target = target;
                        attack.GetComponent<FireSpell>().mage = this;
                        trueDamage += damage * charge;
                        if (target.protectionType == DamageTypes.Magic)
                        {
                            trueDamage -= target.protection;
                        }
                        if (trueDamage < 0)
                        {
                            trueDamage = 0;
                        }
                        charge += 1;
                        if (charge > 10)
                        {
                            charge = 10;
                        }
                        target2 = target;
                        if (target.shield <= 0)
                        {
                            if (target.enemyType != Enemy.EnemyTypes.Boss)
                            {
                                target.ReduceHP(trueDamage);
                            }
                            else
                            {
                                target.ReduceHP(1);
                            }
                        }
                        Destroy(attack, 2f);
                        break;
                    case MageType.Death:
                        MageCrystal.SetActive(true);
                        attack = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
                        attack.GetComponent<DeathSpell>().damage = damage;
                        attack.GetComponent<DeathSpell>().target = target;
                        attack.GetComponent<DeathSpell>().mage = this;
                        break;
                }
                for (float i = 1 / attackSpeed; i > 0; i -= 0.1f)
                {
                    yield return new WaitForSeconds(0.1f);
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
            if (target != null && Vector2.Distance(new Vector2(target.transform.position.x, target.transform.position.z),
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) > attackDistance)
            {
                target = null;
            }
            Enemy[] enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (Enemy enemy in enemyes)
            {
                if (Vector2.Distance(new Vector2(enemy.transform.position.x, enemy.transform.position.z),
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) <= attackDistance)
                {
                    if (mageType == MageType.Death)
                    {
                        if ((target == null) || (enemy.numberPoint > target.numberPoint) ||
                        (Vector2.Distance(new Vector2(enemy.transform.position.x, enemy.transform.position.z),
                    new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) <
                    Vector2.Distance(new Vector2(target.transform.position.x, target.transform.position.z),
                    new Vector2(gameObject.transform.position.x, gameObject.transform.position.z))))
                        {
                            target = enemy;
                        }
                    }
                    else if (mageType == MageType.Fire)
                    {
                        if (target == null)
                        {
                            target = enemy;
                        }
                    }
                    else
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
    public void MageCrystalRecharge(bool b)
    {
        MageCrystal.SetActive(b);
    }
}
