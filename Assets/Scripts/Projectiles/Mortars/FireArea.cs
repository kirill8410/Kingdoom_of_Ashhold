using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FireArea : MonoBehaviour
{
    [SerializeField] float bangDistance;
    [SerializeField] int damage;
    [SerializeField] int seconds;

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
                if (Vector3.Distance(gameObject.transform.position, enemy.gameObject.transform.position) <= bangDistance)
                {
                    enemy.ReduceHP(damage);
                }
            }
        }
        Destroy(gameObject);
    }
}
