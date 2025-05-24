using System;
using UnityEngine;

public class Arrow : MonoBehaviour // Обычная стрела
{
    public Enemy target; // Цель в которую летит стрела
    public float damage; // Урон стрелы
    public float speed = 2; // Скорость стрелы
    public TowerFunctions tower;
    public Transform towerTransform;
    [Header("Effects")]    
    [SerializeField] bool potion;
    [SerializeField] bool sniper;


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
                    if ((enemy.GetNumberPoint() > target.GetNumberPoint()) || ((enemy.GetDistanceToPoint() < target.GetDistanceToPoint()) && (enemy.GetNumberPoint() >= target.GetNumberPoint())))
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
        if (other.gameObject.tag == "Enemy" && other.GetComponent<Enemy>() == target)
        {
            if (sniper)
            {
                if (Vector2.Distance(new Vector2(target.transform.position.x, target.transform.position.z),
                    new Vector2(towerTransform.position.x, towerTransform.position.z)) > 6)
                {
                    damage += Convert.ToInt32(Vector2.Distance(new Vector2(target.transform.position.x, target.transform.position.z),
                    new Vector2(towerTransform.position.x, towerTransform.position.z)) * 2);
                    if (target.GetEnemyType() == Enemy.EnemyTypes.Boss)
                    {
                        damage += 5;
                    }
                }
            }
            if (potion)
            {
                target.Potion(tower.Towerlevel * 10);
            }
            target.ReduceHP(damage);
            Destroy(gameObject);
        }
    }
}
