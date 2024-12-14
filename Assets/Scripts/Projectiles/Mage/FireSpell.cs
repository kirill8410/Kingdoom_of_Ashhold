using System.Runtime.CompilerServices;
using UnityEngine;

public class FireSpell : MonoBehaviour
{
    public Enemy target;
    public Mage mage;

    private void Update()
    {
        if (target != null)
        {
            transform.LookAt(target.gameObject.transform.position);
            GetComponentInChildren<ParticleSystem>().startSpeed = Vector3.Distance(gameObject.transform.position, target.transform.position);
        }
        if (target == null || Vector3.Distance(gameObject.transform.position, target.transform.position) > mage.attackDistance)
        {
            Destroy(gameObject);
        }
    }
}
