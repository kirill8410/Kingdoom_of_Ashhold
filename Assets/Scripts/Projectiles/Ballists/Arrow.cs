using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Enemy target;
    public int damage;
    public float speed;

    private void Update()
    {
        if (target != null)
        {
            transform.LookAt(target.gameObject.transform.position);
            transform.Translate(0, 0, speed * Time.deltaTime * 10f);
        }
        else
        {
            Enemy[] enemyes = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (Enemy enemy in enemyes)
            {
                if ((enemy.numberPoint > target.numberPoint) || ((enemy.distanceToPoint < target.distanceToPoint) && (enemy.numberPoint >= target.numberPoint)))
                {
                    target = enemy;
                }
            }
            if (target == null)
            {
                Destroy(gameObject);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            target.HP -= damage;
            Destroy(gameObject);
        }
    }
}
