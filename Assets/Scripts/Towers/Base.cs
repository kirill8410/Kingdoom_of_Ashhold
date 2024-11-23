using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    private LevelManager LM;
    [SerializeField] GameObject Information;
    [SerializeField] GameObject Selection;

    private void Start()
    {
        LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    public void ShowInformation(TowerData tower)
    {
        if (Information.activeSelf == false)
        {
            Information.SetActive(true);
            Selection.transform.localPosition = new Vector3(80f, 0f, 0f);
        }
        else
        {
            Information.SetActive(false);
            Selection.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
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
