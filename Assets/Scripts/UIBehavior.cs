using System.Collections;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UIBehavior : MonoBehaviour // Поведение UI
{
    // Игрок
    [SerializeField] GameObject player;
    // нужно ли прятать UI если игрок находиться далеко
    [SerializeField] bool isHiding = true;

    private void Start()
    {
        // Поиск игрока на сцене
        player = GameObject.Find("Player");
    }
    private void Update()
    {
        // Поворот UI к игроку
        var direction = (player.transform.position - transform.position).normalized;
        var euler = transform.eulerAngles;
        euler.y = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg - 180;
        transform.eulerAngles = euler; 
        // Появление UI при приблежении игрока
        if (isHiding)
        {
            if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= 6f)
            {
                GetComponent<Canvas>().enabled = true;
            }
            else
            {
                GetComponent<Canvas>().enabled = false;
            }
        }
    }
}
