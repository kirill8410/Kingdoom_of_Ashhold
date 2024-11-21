using UnityEngine;
using System.Collections;
using NUnit.Framework.Constraints;
using static UnityEngine.GraphicsBuffer;

public class Ballista : MonoBehaviour, TowerFunctions // Баллиста
{
    private Tower tower;

    public Enemy target;
    [SerializeField] GameObject Turet_osnov;

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

    public IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f/tower.attackSpeed);
            if ((target != null) && (tower.isAttack))
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
                target.HP -= trueDamage;
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

    private void RotationTuret()
    {
        Turet_osnov.transform.LookAt(target.gameObject.transform.position);
        Turet_osnov.transform.rotation = new Quaternion(0f, Turet_osnov.transform.rotation.y, 0f, Turet_osnov.transform.rotation.w);
    }
}
