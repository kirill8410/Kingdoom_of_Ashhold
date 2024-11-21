using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Enemy target;
    public int damage;
    public float speed;

    private void Update()
    { if (target != null)
        {
            transform.LookAt(target.gameObject.transform.position);
            //transform.localPosition += new Vector3();
        }

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
