using System.Runtime.CompilerServices;
using UnityEngine;

public class DeathSpell : MonoBehaviour
{
    public Enemy Target; 
    public Mage mage;

    private void Start()
    {
        transform.LookAt(new Vector3(Target.gameObject.transform.position.x, gameObject.transform.position.y,
            Target.gameObject.transform.position.z));
    }
    private void Update()
    {
        if (Vector2.Distance(new Vector2(gameObject.transform.position.x, gameObject.transform.position.z),
            new Vector2(mage.transform.position.x, mage.transform.position.z)) < mage.GetAttackDistance())
        {
            transform.Translate(0, 0, 15 * Time.deltaTime);
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
            other.GetComponent<Enemy>().Curse(, damage);
        }
    }
}
