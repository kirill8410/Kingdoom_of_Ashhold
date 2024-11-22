using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Enemy target;
    public int damage;
    public float speed;

    private void Update()
    { 
        this.transform.LookAt(target.gameObject.transform.position);
        this.transform.Translate(0, 0, speed * Time.deltaTime * 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            target.HP -= damage;
            Destroy(gameObject);
        }
    }
}
