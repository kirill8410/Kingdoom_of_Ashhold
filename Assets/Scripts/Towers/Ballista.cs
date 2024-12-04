using UnityEngine;
using System.Collections;
using NUnit.Framework.Constraints;
using static UnityEngine.GraphicsBuffer;

public class Ballista : Tower, TowerFunctions // Баллиста
{
    public int PriceLevelUp { get; set; }
    public int Towerlevel { get; set; }

    [Header("Turets")]
    [SerializeField] GameObject Turet_osnov;
    [SerializeField] GameObject Turet_osnov2;

    [Header("TowerType")]
    [SerializeField] bool isPoison;
    [SerializeField] bool isDouble;

    private GameObject arrow;
    private int trueDamage;

    public Enemy target;

    private void Start()
    {
        StartCoroutine(SearchTarget());
        StartCoroutine(Attack());
    }
    private void Update()
    {
        if (target != null)
        {
            RotationTuret();
        }
    }
    public void ArrowSpawn()
    {
        arrow.SetActive(true);
    }

    public IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f/attackSpeed);
            if ((target != null) && (isAttack) && Vector3.Distance(gameObject.transform.position,
                target.gameObject.transform.position) <= attackDistance)
            {
                GetComponent<Animator>().SetTrigger("Attack");

                trueDamage = damage;
                if (damageType == target.protectionType)
                {
                    trueDamage -= target.protection;
                }
                if (trueDamage < 0)
                {
                    trueDamage = 0;
                }
                GameObject attack = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
                attack.GetComponent<Arrow>().damage = trueDamage;
                attack.GetComponent<Arrow>().target = target;
                arrow = attack;
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
            PriceLevelUp = levelUp.priceLevelUp_1;
            Towerlevel = 2;
        }
        else if (Towerlevel == 2)
        {
            damage += levelUp.damage_2;
            attackDistance += levelUp.distance_2;
            PriceLevelUp = levelUp.priceLevelUp_2;
            Towerlevel = 3;
        }
    }

    private void RotationTuret()
    {
        Turet_osnov.transform.LookAt(target.gameObject.transform.position);
        Turet_osnov2.transform.LookAt(new Vector3(target.gameObject.transform.position.x, Turet_osnov2.transform.position.y, target.gameObject.transform.position.z));
    }
}
