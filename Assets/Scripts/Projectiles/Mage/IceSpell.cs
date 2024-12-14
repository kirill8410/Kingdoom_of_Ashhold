using UnityEngine;

public class IceSpell : MonoBehaviour
{
    public Enemy target; // Цель в которую летит магия
    public int damage; // Урон магии
    public float speed = 2; // Скорость магии
    public Mage mage;

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
            target.ReduceHP(damage);
            mage.MageCrystalRecharge(true);
            target.Ice();
            Destroy(gameObject);
        }
    }
}
