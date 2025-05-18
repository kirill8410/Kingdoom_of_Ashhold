using UnityEngine;

public class EffectTimer : MonoBehaviour
{
    public float time;
    public float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time <= (timer += Time.deltaTime))
        {
            Destroy(gameObject);
        }
    }
}
