using UnityEngine;

public class fire : MonoBehaviour
{
    float trueDamage;
    float damage;
    public Traps fireTrap;
     // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        damage = fireTrap._damage;   
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<Enemy>() != null)
        {
            
            if (trueDamage < 0)
            {
                trueDamage = 0;
            }
            other.GetComponent<Enemy>().ReduceHP(trueDamage);
        }
    }
}
