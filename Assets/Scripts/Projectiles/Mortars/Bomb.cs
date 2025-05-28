using UnityEngine;
using static Tower;
using static UnityEngine.GraphicsBuffer;

public class Bomb : MonoBehaviour
{
    public Mortar Mortar;

    public void Bang()
    {
        Enemy[] enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemyes)
        {
            if (Vector2.Distance(new Vector2(gameObject.transform.position.x, gameObject.transform.position.z), 
                new Vector2(enemy.transform.position.x, enemy.transform.position.z)) <= transform.localScale.x)
            {
                enemy.ReduceHP(Mortar.GetDamage(), Mortar.GetDamageType(), Mortar.GetBreakingProtection());
            }
        }
        Destroy(gameObject, 1f);
    }
}
