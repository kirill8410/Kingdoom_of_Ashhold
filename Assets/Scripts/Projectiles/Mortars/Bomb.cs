using UnityEngine;
using static Tower;
using static UnityEngine.GraphicsBuffer;

public class Bomb : MonoBehaviour
{
    public int damage;
    public float bangDistance;
    private int trueDamage;

    public void Bang()
    {
        Enemy[] enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemyes)
        {
            if (Vector2.Distance(new Vector2(gameObject.transform.position.x, gameObject.transform.position.z), 
                new Vector2(enemy.transform.position.x, enemy.transform.position.z)) <= bangDistance)
            {
                trueDamage = damage;
                if (enemy.protectionType == DamageTypes.Physical)
                {
                    trueDamage -= enemy.protection * 2;
                }
                else
                {
                    trueDamage -= enemy.protection;
                }
                if (trueDamage < 0)
                {
                    trueDamage = 0;
                }
                enemy.ReduceHP(trueDamage);
            }
        }
        Destroy(gameObject, 1f);
    }
}
