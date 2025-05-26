using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Traps : Tower, TowerFunctions
{
    [SerializeField] GameObject effect;
    public bool attackEffect;
    public bool isAttack { get; set; } = true;
    public GameObject gm { get; set; }

    [Header("Level")]

    public int PriceLevelUp { get; set; }
    public int Towerlevel { get; set; } = 1;

    [SerializeField] LevelUp _levelUp;
    [SerializeField] GameObject Turet_osnov;
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


    [Header("TrapType")]

    [SerializeField] TrapType trapType;
    private enum TrapType
    {
        Simple, Fire, Mine,
    }

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
        if (trapType == TrapType.Simple)
        {
            effect.SetActive(attackEffect);
        }
        RotationTuret();
        _distancePrefab.transform.localScale = new Vector3(_attackDistance * 2f, _attackDistance * 2f, 1f);
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
            yield return new WaitForSeconds(1f / _attackSpeed);
            if ((target != null) && (isAttack) && Vector3.Distance(gameObject.transform.position,
                target.gameObject.transform.position) <= _attackDistance)
            {
                switch (trapType)
                {
                    case TrapType.Simple:
                        
                        attackEffect = true;
                        _attackPrefab.SetActive(!_attackPrefab.activeSelf); break; 
                        
                        
                }
            }
            else
            {
                attackEffect = false;
            }
        }
    }
    public IEnumerator SearchTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (target != null && Vector3.Distance(target.transform.position, gameObject.transform.position) > _attackDistance)
            {
                target = null;
            }
            Enemy[] enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (Enemy enemy in enemyes)
            {
                if (Vector3.Distance(gameObject.transform.position, enemy.transform.position) <= _attackDistance)
                {
                    if ((target == null) || (enemy.GetNumberPoint() > target.GetNumberPoint()) ||
                        ((enemy.GetDistanceToPoint() < target.GetDistanceToPoint()) && (enemy.GetNumberPoint() >= target.GetNumberPoint())))
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
            _damage += levelUp.damage_1;
            _attackDistance += levelUp.distance_1;
            Towerlevel = 2;
        }
        else if (Towerlevel == 2)
        {
            _damage += levelUp.damage_2;
            _attackDistance += levelUp.distance_2;
            Towerlevel = 3;
        }
    }
    private void RotationTuret()
    {
        if (target != null)
            Turet_osnov.transform.LookAt(target.gameObject.transform.position);
    }
}