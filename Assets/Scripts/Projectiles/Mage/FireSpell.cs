using System.Runtime.CompilerServices;
using UnityEngine;

public class FireSpell : MonoBehaviour
{
    public Enemy target;
    public float speed = 2;

    private void Update()
    {
        if (target != null)
        {
            transform.LookAt(target.gameObject.transform.position);
            transform.Translate(0, 0, speed * Time.deltaTime * 10f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) // Ќанесение урона при поподании по врагу
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject, 1f);
        }
    }
}
