using UnityEngine;
using static Tower;

public class FireBomb : MonoBehaviour
{
    public Mortar Mortar;
    public int fireSeconds;
    [SerializeField] GameObject fireArea;

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
        GameObject Fire = Instantiate(fireArea, new Vector3(gameObject.transform.position.x, 0f, 
            gameObject.transform.position.z), gameObject.transform.rotation);
        Fire.GetComponent<FireArea>().seconds = fireSeconds;
        Destroy(gameObject, 1f);
    }
}
