using System;
using UnityEngine;
using static Tower;

public class Spell : MonoBehaviour
{
    public Enemy target; // Цель в которую летит магия
    public int damage; // Урон магии
    public float speed = 2; // Скорость магии
    public Mage mage;
    bool isBang = false;
    private int trueDamage;

    private void Update()
    {
        if (target != null) // Движение магии к цели
        {
            transform.LookAt(target.gameObject.transform.position);
            transform.Translate(0, 0, speed * Time.deltaTime * 10f);
        }
        else 
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
                trueDamage = damage;
                if (other.GetComponent<Enemy>().protectionType == DamageTypes.Magic)
                {
                    trueDamage -= target.protection;
                }
                if (trueDamage < 0)
                {
                    trueDamage = 0;
                }
                other.gameObject.GetComponent<Enemy>().ReduceHP(trueDamage);
                mage.MageCrystalRecharge(true);
                Destroy(gameObject);
            }
        }
    }
}
