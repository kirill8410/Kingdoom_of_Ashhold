using UnityEngine;
using System.Collections;
using NUnit.Framework.Constraints;

public class Ballista : MonoBehaviour, TowerFunctions // Баллиста
{
    private Tower tower;

    public Enemy target;

    private int trueDamage;
    private float trueAttackSpeed;

    private void Start()
    {
        tower = GetComponent<Tower>();
        StartCoroutine(SearchTarget());
        StartCoroutine(Attack());
    }

    public IEnumerator Attack()
    {
        while (true)
        {
            trueAttackSpeed = tower.attackSpeed + tower.attackAcceleration - tower.attackSlowdown;
            if (trueAttackSpeed < 0 )
            {
                trueAttackSpeed = 0.001f;
            }
            yield return new WaitForSeconds(1f/trueAttackSpeed);
            if ((target != null) && (tower.isAttack))
            {
                trueDamage = tower.damage - tower.damageReduction + tower.damegeIncrease;
                if (tower.damageType == target.protectionType)
                {
                    trueDamage -= target.protection;
                }
                trueDamage += Random.Range(-1*(trueDamage/5), trueDamage/5);
                if (trueDamage < 0)
                {
                    trueDamage = 0;
                }
                target.HP -= trueDamage;
                print("Пиу");
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
                    if ((target == null) || (enemy.distanceToPoint < target.distanceToPoint))
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
            tower.attackSpeed += 0.3f;
            tower.price = 120;
            tower.level = 3;
        }
    }
}
