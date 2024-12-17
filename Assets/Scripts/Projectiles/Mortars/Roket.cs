using System.Runtime.CompilerServices;
using UnityEngine;

public class Roket : MonoBehaviour
{
    public Enemy target;
    public int damage;
    public float bangDistance;
    public float speed = 1.5f;

    private void Update()
    {
        if (target != null)
        {
            transform.LookAt(target.gameObject.transform.position);
            transform.Translate(0, 0, speed * Time.deltaTime * 10f);
        }
        else
        {
            Bang();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Bang();
        }
    }

    public void Bang()
    {
        Enemy[] enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemyes)
        {
            if (Vector3.Distance(gameObject.transform.position, enemy.gameObject.transform.position) <= bangDistance)
            {
                enemy.ReduceHP(damage);
            }
        }
        Destroy(gameObject, 0.5f);
    }
}
