using UnityEngine;

public class Base : MonoBehaviour
{
    private LevelManager LM;
    [SerializeField] GameObject Information;

    private void Start()
    {
        LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();   
    }

    public void ShowInformation(TowerData tower)
    {

    }
    public void Build(TowerData tower)
    {
        if (LM.coins >= tower.price)
        {
            LM.coins -= tower.price;
            Instantiate(tower.tower);
            Destroy(gameObject);
        }
    }
}
