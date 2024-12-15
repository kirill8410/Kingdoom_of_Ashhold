using System.Runtime.CompilerServices;
using UnityEngine;

public class DeathSpell : MonoBehaviour
{
    public Enemy target; 
    public int damage; 
    public float speed = 4f; 
    public Mage mage;

    private void Start()
    {
        transform.LookAt(target.gameObject.transform.position);
    }
    private void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, mage.transform.position) < mage.attackDistance)
        {
            transform.Translate(0, 0, speed * Time.deltaTime * 10f);
        }
        else
        {
            mage.MageCrystalRecharge(false);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<Enemy>().ReduceHP(damage);
            other.GetComponent<Enemy>().Curse(mage.damage/5);
        }
    }
}
