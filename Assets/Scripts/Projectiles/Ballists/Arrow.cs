using UnityEngine;
using UnityEngine.XR.OpenXR.Features;

public class Arrow : MonoBehaviour // Обычная стрела
{
    public Enemy target; // Цель в которую летит стрела
    public int damage; // Урон стрелы
    public float speed; // Скорость стрелы

    private void Update()
    {
        if (target != null) // Движение стрелы к цели
        {
            transform.LookAt(target.gameObject.transform.position);
            transform.Translate(0, 0, speed * Time.deltaTime * 10f);
        }
        else // Поиск цели если она отсутствует 
        {
            Enemy[] enemyes = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
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
            target.ReduceHP(damage);
            print(damage);
            Destroy(gameObject);
        }
    }
}
