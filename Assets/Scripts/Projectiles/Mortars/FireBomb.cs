using UnityEngine;

public class FireBomb : MonoBehaviour
{
    public int damage;
    public float bangDistance;
    [SerializeField] GameObject fireArea;
    private int trueDamage;

    public void Bang()
    {
        Enemy[] enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemyes)
        {
            if (Vector3.Distance(gameObject.transform.position, enemy.gameObject.transform.position) <= bangDistance)
            {
                trueDamage = damage;
                if (enemy.protectionType == Tower.DamageTypes.Physical)
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
        Instantiate(fireArea, new Vector3(gameObject.transform.position.x, 0f, gameObject.transform.position.z), gameObject.transform.rotation);
        Destroy(gameObject, 1f);
    }
}
