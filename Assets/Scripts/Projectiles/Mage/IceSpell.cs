using UnityEngine;
using static Tower;

public class IceSpell : MonoBehaviour
{
    public Enemy target; // Цель в которую летит магия
    public float damage; // Урон магии
    public float speed = 2; // Скорость магии
    public float slow = 0.4f;

    private void Update()
    {
        if (target != null) // Движение магии к цели
        {
            transform.LookAt(target.gameObject.transform.position);
            transform.Translate(0, 0, speed * Time.deltaTime * 10f);
        }
        else // Поиск цели если она отсутствует 
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) // Нанесение урона при поподании по врагу
    {
        if (other.gameObject.tag == "Enemy" && other.GetComponent<Enemy>() == target)
        {
            target.ReduceHP(damage);
            target.Ice(slow);
            Destroy(gameObject);
        }
    }
}
