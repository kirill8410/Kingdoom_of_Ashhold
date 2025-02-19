using UnityEngine;

public class fire : MonoBehaviour
{
    int trueDamage;
    int damage;
    public Traps fireTrap;
     // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        damage = fireTrap.damage;   
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<Enemy>() != null)
        {
            trueDamage = damage;
            if (other.GetComponent<Enemy>().protectionType == Tower.DamageTypes.Physical)
            {
                trueDamage -= other.GetComponent<Enemy>().protection;
            }
            if (trueDamage < 0)
            {
                trueDamage = 0;
            }
            other.GetComponent<Enemy>().ReduceHP(trueDamage);
        }
    }
}
