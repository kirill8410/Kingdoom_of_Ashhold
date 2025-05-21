using System.Runtime.CompilerServices;
using UnityEngine;

public class DeathSpell : MonoBehaviour
{
    public Enemy target; 
    public float damage; 
    private float speed = 15f; 
    public Mage mage;

    private void Start()
    {
        transform.LookAt(new Vector3(target.gameObject.transform.position.x, gameObject.transform.position.y,
            target.gameObject.transform.position.z));
    }
    private void Update()
    {
        if (Vector2.Distance(new Vector2(gameObject.transform.position.x, gameObject.transform.position.z),
            new Vector2(mage.transform.position.x, mage.transform.position.z)) < mage.attackDistance)
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
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
            other.GetComponent<Enemy>().Curse(damage, damage);
        }
    }
}
