using UnityEngine;
using static Tower;

public class FireBomb : MonoBehaviour
{
    public int damage;
    public int fireDamage;
    public int fireSeconds;
    public float bangDistance;
    [SerializeField] GameObject fireArea;
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
        GameObject Fire = Instantiate(fireArea, new Vector3(gameObject.transform.position.x, 0f, gameObject.transform.position.z), gameObject.transform.rotation);
        Fire.GetComponent<FireArea>().damage = fireDamage;
        Fire.GetComponent<FireArea>().seconds = fireSeconds;
        Destroy(gameObject, 1f);
    }
}
