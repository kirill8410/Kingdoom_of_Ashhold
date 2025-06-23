using System;
using UnityEngine;
using static Tower;

public class Spell : MonoBehaviour
{
    public Enemy Target;
    public Mage Mage;

    private void Update()
    {
        if (Target != null) // Движение магии к цели
        {
            transform.LookAt(Target.gameObject.transform.position);
            transform.Translate(0, 0, Time.deltaTime * 30f);
        }
    }

    private void OnTriggerEnter(Collider other) // Нанесение урона при поподании по врагу
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject == Target.gameObject)
        {
            Target.ReduceHP(Mage.GetDamage(), Mage.GetDamageType(), Mage.GetBreakingProtection());
            Mage.MageCrystalRecharge(true);
            Destroy(gameObject);
        }
    }
}
