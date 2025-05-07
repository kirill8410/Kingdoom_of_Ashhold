using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FireArea : MonoBehaviour
{
    float bangDistance = 4f;
    float damage = 10;
    public int seconds;

    private void Start()
    {
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        for (int i = 0; i < seconds * 2; i++)
        {
            yield return new WaitForSeconds(0.5f);
            Enemy[] enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (Enemy enemy in enemyes)
            {
                if (Vector2.Distance(new Vector2(gameObject.transform.position.x, gameObject.transform.position.z),
                new Vector2(enemy.transform.position.x, enemy.transform.position.z)) <= bangDistance)
                {
                    float trueDamage = damage;
                    if (enemy.protectionType == Tower.DamageTypes.Magic)
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
        }
        Destroy(gameObject);
    }
}
