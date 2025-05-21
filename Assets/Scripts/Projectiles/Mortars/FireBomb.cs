using UnityEngine;
using static Tower;

public class FireBomb : MonoBehaviour
{
    public float damage;
    public int fireSeconds;
    public float bangDistance;
    [SerializeField] GameObject fireArea;

    public void Bang()
    {
        Enemy[] enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemyes)
        {
            if (Vector2.Distance(new Vector2(gameObject.transform.position.x, gameObject.transform.position.z), 
                new Vector2(enemy.transform.position.x, enemy.transform.position.z)) <= bangDistance)
            {
                enemy.ReduceHP(damage);
            }
        }
        GameObject Fire = Instantiate(fireArea, new Vector3(gameObject.transform.position.x, 0f, 
            gameObject.transform.position.z), gameObject.transform.rotation);
        Fire.GetComponent<FireArea>().seconds = fireSeconds;
        Destroy(gameObject, 1f);
    }
}
