using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public int health = 10;
    public int maxhealth;
    public int ironhealth = 10;
    public int maxironhealth;
    public GameObject[] health_points;
    public GameObject[] ironhealth_points;
    public GameObject ironhealth_icon;
    public GameObject ironkinghealth_icon;
    public GameObject kinghealth_icon;
    public bool iron;
    public bool king;
    public GameObject enemy;
    public Enemy script;

    private void Start()
    {
        maxhealth = health;
        script = enemy.GetComponent<Enemy>();
    }
    private void Update()
    {
       if(health < maxhealth)
       {           
            Health();
            maxhealth -= 1;
       }
       if (health > maxhealth)
       {
           Health1();
           maxhealth += 1;
       }
        if (iron)
        {
            if (king)
            {
                ironkinghealth_icon.SetActive(true);
            }
            else
            {
                ironhealth_icon.SetActive(true);
            }
            
            if (ironhealth < maxironhealth)
            {
                IronHealth();
                maxironhealth -= 1;
            }
            if (ironhealth > maxironhealth)
            {
                IronHealth1();
                maxironhealth += 1;
            }
        }
        else if (king)
        {
            kinghealth_icon.SetActive(true);
        }
    }

    private void Health()
    {
        health_points[maxhealth-1].SetActive(false);
    }
    private void Health1()
    {
        
        health_points[maxhealth].SetActive(true);
        
    }
    private void IronHealth()
    {
        ironhealth_points[maxironhealth - 1].SetActive(false);
    }
    private void IronHealth1()
    {

        ironhealth_points[maxironhealth].SetActive(true);

    }
}
