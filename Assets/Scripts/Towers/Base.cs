using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    private LevelManager LM;

    [Header("Windows")]
    [SerializeField] GameObject Information;
    [SerializeField] GameObject Selection;

    [Header("Texts")]
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI Description;
    [SerializeField] TextMeshProUGUI Price;
    [SerializeField] TextMeshProUGUI Damage;
    [SerializeField] TextMeshProUGUI Distance;
    [SerializeField] TextMeshProUGUI AttackSpeed;

    [Header("Images")]
    [SerializeField] Image MageDamage;
    [SerializeField] Image PhysicalDamage;

    private void Start()
    {
        LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    public void ShowInformation(TowerData tower)
    {
        if (Information.activeSelf == false)
        {
            Information.SetActive(true);
            Selection.transform.localPosition = new Vector3(-80f, 0f, 0f);

            Description.text = tower.description;
            Name.text = tower.TowerName;
            Price.text = tower.price.ToString();
            Damage.text = tower.tower.GetComponent<Tower>().damage.ToString();
            Distance.text = (tower.tower.GetComponent<Tower>().attackDistance / 5).ToString();
            AttackSpeed.text = tower.tower.GetComponent<Tower>().attackSpeed.ToString();
            if (tower.tower.GetComponent<Tower>().damageType == Tower.DamageTypes.Physical)
            {
                MageDamage.enabled = false;
                PhysicalDamage.enabled = true;
            }
            else
            {
                MageDamage.enabled = true;
                PhysicalDamage.enabled = false;
            }
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
