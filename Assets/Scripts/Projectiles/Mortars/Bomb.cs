using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int damage;
    public float bangDistance;

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
        Destroy(gameObject, 1f);
    }
}
