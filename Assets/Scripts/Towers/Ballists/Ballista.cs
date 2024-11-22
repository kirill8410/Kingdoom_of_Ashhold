using UnityEngine;
using System.Collections;
using NUnit.Framework.Constraints;
using static UnityEngine.GraphicsBuffer;

public class Ballista : MonoBehaviour, TowerFunctions // Баллиста
{
    private Tower tower;

    public Enemy target;

    [SerializeField] GameObject Turet_osnov;
    [SerializeField] GameObject Turet_osnov2;
    private GameObject arrow;
    private int trueDamage;

    private void Start()
    {
        tower = GetComponent<Tower>();
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
            yield return new WaitForSeconds(1f/tower.attackSpeed);
            if ((target != null) && (tower.isAttack) && Vector3.Distance(gameObject.transform.position,
                target.gameObject.transform.position) <= tower.attackDistance)
            {
                GetComponent<Animator>().SetTrigger("Attack");

                trueDamage = tower.damage - tower.damageReduction + tower.damegeIncrease;
                if (tower.damageType == target.protectionType)
                {
                    trueDamage -= target.protection;
                }
                if (trueDamage < 0)
                {
                    trueDamage = 0;
                }
                GameObject attack = Instantiate(tower.attackPrefab, tower.attackPoint.position, tower.attackPoint.rotation);
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
                if (Vector3.Distance(gameObject.transform.position, enemy.transform.position) <= tower.attackDistance)
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
        if (tower.level == 1)
        {
            tower.damage += 5;
            tower.price = 85;
            tower.level = 2;
        }
        else if (tower.level == 2) 
        {
            tower.damage += 5;
            tower.price = 120;
            tower.level = 3;
        }
    }

    private void RotationTuret()
    {
        Turet_osnov.transform.LookAt(target.gameObject.transform.position);
        Turet_osnov2.transform.LookAt(new Vector3(target.gameObject.transform.position.x, Turet_osnov2.transform.position.y, target.gameObject.transform.position.z));
    }
}
