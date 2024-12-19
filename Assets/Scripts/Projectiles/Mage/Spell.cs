using System;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public Enemy target; // Цель в которую летит магия
    public int damage; // Урон магии
    public float speed = 2; // Скорость магии
    public Mage mage;
    bool isBang;

    private void Update()
    {
        if (target != null) // Движение магии к цели
        {
            transform.LookAt(target.gameObject.transform.position);
            transform.Translate(0, 0, speed * Time.deltaTime * 10f);
        }
        else // Поиск цели если она отсутствует 
        {
            if (!isBang)
            {
                Enemy[] enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
                foreach (Enemy enemy in enemyes)
                {
                    if (Vector3.Distance(gameObject.transform.position, enemy.gameObject.transform.position) <= 3f)
                    {
                        enemy.ReduceHP(damage * 2);
                    }
                }
            }
            isBang = true;
            transform.localScale = new Vector3
            (
            transform.localScale.x + 5f, transform.localScale.y + 5f, transform.localScale.z + 5f
            );
            mage.MageCrystalRecharge(true);
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnTriggerEnter(Collider other) // Нанесение урона при поподании по врагу
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (!isBang)
            {
                other.GetComponent<Enemy>().ReduceHP(damage);
                mage.MageCrystalRecharge(true);
                Destroy(gameObject);
            }
        }
    }
}
