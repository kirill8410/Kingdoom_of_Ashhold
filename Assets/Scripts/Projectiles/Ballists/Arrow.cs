using System;
using UnityEngine;

public class Arrow : MonoBehaviour // Обычная стрела
{
    public Ballista Ballista;
    public Enemy Target;


    private void Update()
    {
        if (Target != null) // Движение стрелы к цели
        {
            transform.LookAt(Target.gameObject.transform.position);
            transform.Translate(0, 0, Time.deltaTime * 30f);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) // Нанесение урона при поподании по врагу
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (Ballista.GetTowerType() == Tower.TowerTypes.SniperBallist)
            {
                if (Vector2.Distance(new Vector2(Target.transform.position.x, Target.transform.position.z),
                    new Vector2(Ballista.transform.position.x, Ballista.transform.position.z)) > 6)
                {
                    if (Target.GetEnemyType() == Enemy.EnemyTypes.Boss)
                    {
                        Target.ReduceHP((5 + Ballista.GetDamage() + 
                            Vector2.Distance(new Vector2(Target.transform.position.x, Target.transform.position.z),
                            new Vector2(Ballista.transform.position.x, Ballista.transform.position.z)) * 2), 
                            Ballista.GetDamageType(), Ballista.GetBreakingProtection());
                    }
                    else
                    {
                        Target.ReduceHP((Ballista.GetDamage() + 
                            Vector2.Distance(new Vector2(Target.transform.position.x, Target.transform.position.z),
                            new Vector2(Ballista.transform.position.x, Ballista.transform.position.z)) * 2), 
                            Ballista.GetDamageType(), Ballista.GetBreakingProtection());
                    }
                }
            }
            else if (Ballista.GetTowerType() == Tower.TowerTypes.PoisonBallist)
            {
                Target.Potion(Ballista.TowerLevel * 10);
                Target.ReduceHP(Ballista.GetDamage(), Ballista.GetDamageType());
            }
            Destroy(gameObject);
        }
    }
}
