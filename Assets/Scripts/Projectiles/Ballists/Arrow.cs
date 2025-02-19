using System;
using UnityEngine;

public class Arrow : MonoBehaviour // Обычная стрела
{
    public Enemy target; // Цель в которую летит стрела
    public int damage; // Урон стрелы
    public float speed = 2; // Скорость стрелы
    public TowerFunctions tower;
    public Transform towerTransform;
    [Header("Effects")]    
    [SerializeField] bool potion;
    [SerializeField] bool sniper;
    private int trueDamage;


    private void Update()
    {
        if (target != null) // Движение стрелы к цели
        {
            transform.LookAt(target.gameObject.transform.position);
            transform.Translate(0, 0, speed * Time.deltaTime * 10f);
        }
        else // Поиск цели если она отсутствует 
        {
            Enemy[] enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (Enemy enemy in enemyes)
            {
                if (Vector3.Distance(gameObject.transform.position, enemy.gameObject.transform.position) <= 3f)
                {
                    if ((enemy.numberPoint > target.numberPoint) || ((enemy.distanceToPoint < target.distanceToPoint) && (enemy.numberPoint >= target.numberPoint)))
                    {
                        target = enemy;
                    }
                }
            }
            if (target == null) // Удаление стрелы если врагов на корте не осталось
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other) // Нанесение урона при поподании по врагу
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (sniper)
            {
                damage += Convert.ToInt32(Vector3.Distance(gameObject.transform.position, towerTransform.position) * 2);
                if (target.enemyType == Enemy.EnemyTypes.Boss)
                {
                    damage += 5;
                }
            }
            if (potion)
            {
                target.Potion(tower.Towerlevel + 2);
            }
            trueDamage = damage;
            if (other.GetComponent<Enemy>().protectionType == Tower.DamageTypes.Physical)
            {
                trueDamage -= target.protection;
            }
            if (trueDamage < 0)
            {
                trueDamage = 0;
            }
            other.gameObject.GetComponent<Enemy>().ReduceHP(trueDamage);
            Destroy(gameObject);
        }
    }
}
