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
            if (Vector3.Distance(gameObject.transform.position, enemy.gameObject.transform.position) <= bangDistance)
            {
                trueDamage = damage;
                if (enemy.protectionType == DamageTypes.Physical)
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
