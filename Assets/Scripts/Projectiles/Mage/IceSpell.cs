using UnityEngine;
using static Tower;

public class IceSpell : MonoBehaviour
{
    public Enemy target; // Цель в которую летит магия
    public int damage; // Урон магии
    public float speed = 2; // Скорость магии
    public Mage mage;
    private int trueDamage;

    private void Update()
    {
        if (target != null) // Движение магии к цели
        {
            transform.LookAt(target.gameObject.transform.position);
            transform.Translate(0, 0, speed * Time.deltaTime * 10f);
        }
        else // Поиск цели если она отсутствует 
        {
            mage.MageCrystalRecharge(true);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) // Нанесение урона при поподании по врагу
    {
        if (other.gameObject.tag == "Enemy")
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
            other.GetComponent<Enemy>().Ice();
            Destroy(gameObject);
        }
    }
}
