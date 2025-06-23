using UnityEngine;

public class IceSpell : MonoBehaviour
{
    public Enemy Target;
    public Mage Mage;
    public float slow = 0.4f;

    private void Update()
    {
        if (Target != null) // Движение магии к цели
        {
            transform.LookAt(Target.gameObject.transform.position);
            transform.Translate(0, 0, Time.deltaTime * 20f);
        }
        else // Поиск цели если она отсутствует 
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) // Нанесение урона при поподании по врагу
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject == Target.gameObject)
        {
            Target.ReduceHP(Mage.GetDamage(), Mage.GetDamageType(), Mage.GetBreakingProtection());
            Target.Ice(slow);
            Destroy(gameObject);
        }
    }
}
