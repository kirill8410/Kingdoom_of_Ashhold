using UnityEngine;
using System.Collections;
using NUnit.Framework.Constraints;
using static UnityEngine.GraphicsBuffer;

public class Ballista : Tower, TowerFunctions // Баллиста
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

    [Header("Turets")]
    [SerializeField] GameObject Turet_osnov;
    [SerializeField] GameObject Turet_osnov2;

    [Header("TowerType")]
    [SerializeField] bool isSniper;
    [SerializeField] bool isDouble;

    private GameObject _arrow1;
    private GameObject _arrow2;
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
        if (target != null)
        {
            RotationTuret();
        }
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
    public void ArrowSpawn(int arrow)
    {
        
        if (arrow == 1)
        {
            _arrow1.SetActive(true);
        }
        else if (arrow == 2)
        {
            _arrow2.SetActive(true);
        }
    }

    public IEnumerator Attack()
    {
        while (true)
        {
            for (float i = 1/attackSpeed; i > 0; i -= 0.1f)
            {
                yield return new WaitForSeconds(0.1f);
            }
            if ((target != null) && (isAttack) && Vector3.Distance(gameObject.transform.position,
                target.gameObject.transform.position) <= attackDistance)
            {
                GetComponent<Animator>().SetTrigger("Attack");

                GameObject attack = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
                attack.GetComponent<Arrow>().damage = damage;
                attack.GetComponent<Arrow>().target = target;
                attack.GetComponent<Arrow>().tower = this;
                _arrow1 = attack;

                if (isDouble)
                {
                    GameObject attack1 = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
                    attack1.GetComponent<Arrow>().damage = damage;
                    attack1.GetComponent<Arrow>().target = target;
                    _arrow2 = attack1;
                }
                if (isSniper)
                {
                    attack.GetComponent<Arrow>().towerTransform = gameObject.transform;
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
        Turet_osnov.transform.LookAt(target.gameObject.transform.position);
        Turet_osnov2.transform.LookAt(new Vector3(target.gameObject.transform.position.x, Turet_osnov2.transform.position.y, target.gameObject.transform.position.z));
    }
}
