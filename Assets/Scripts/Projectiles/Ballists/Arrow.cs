using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Enemy target;
    public int damage;
    public float speed;

    private void Update()
    {
        transform.LookAt(target.gameObject.transform.position);
        transform.Translate(Vector3.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            target.HP -= damage;
            Destroy(gameObject);
        }
    }
}
