using System.Runtime.CompilerServices;
using UnityEngine;

public class DeathSpell : MonoBehaviour
{
    public Enemy Target; 
    public Mage Mage;

    private void Start()
    {
        transform.LookAt(new Vector3(Target.gameObject.transform.position.x, gameObject.transform.position.y,
            Target.gameObject.transform.position.z));
    }
    private void Update()
    {
        if (Vector2.Distance(new Vector2(gameObject.transform.position.x, gameObject.transform.position.z),
            new Vector2(Mage.transform.position.x, Mage.transform.position.z)) < Mage.GetAttackDistance())
        {
            transform.Translate(0, 0, 15 * Time.deltaTime);
        }
        else
        {
            Mage.MageCrystalRecharge(false);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<Enemy>().Curse(Mage.GetDamage(), 5);
        }
    }
}
