using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    // Менеджеры
    private LevelManager LM;
    private SoundManager SM;

    private TowerData selectTower;
    private TowerFunctions _tower;

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

    [Header("Button")]
    [SerializeField] Button BuildButton;
    [SerializeField] Button[] EvolutionButtons;
    [SerializeField] Button LevelUpButton;

    [Header("Audio")]
    [SerializeField] AudioClip Buy;
    [SerializeField] AudioClip Error;

    [Header("GameObject")]
    [SerializeField] GameObject _base;

    private void Start() 
    {
        // Нахождение менеджеров
        LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        SM = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        // Нахождение башни на которой находится этот UI
        _tower = GetComponentInParent<TowerFunctions>();
    }

    private void Update()
    {
        if (_tower != null)
        {
            if (_tower.Towerlevel != 3)
            {
                LevelUpButton.gameObject.SetActive(true);
                foreach (Button EvolutionButton in EvolutionButtons)
                {
                    EvolutionButton.gameObject.SetActive(false);
                }
            }
            else
            {
                LevelUpButton.gameObject.SetActive(false);
                foreach (Button EvolutionButton in EvolutionButtons)
                {
                    EvolutionButton.gameObject.SetActive(true);
                }
            }
        }
    }

    public void ShowInformation(TowerData tower) // Паказываем информацию о башне
    {
        if (Information.activeSelf == false || selectTower != tower) // Показать информацию если информация не показана или показана о другой башне
        {
            selectTower = tower;

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
        else if ((Information.activeSelf == false && selectTower == tower) || GetComponent<Canvas>().enabled == false) // Скрыть информацию
        {
            selectTower = null;
            Information.SetActive(false);
            Selection.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }
    public void Build() // Построить башню
    {
        if (_base != null)
        {
            if (LM.coins >= selectTower.price)
            {
                GetComponent<AudioSource>().clip = Buy;
                SM.PlaySound(GetComponent<AudioSource>());

                LM.coins -= selectTower.price;
                Instantiate(selectTower.tower, _base.transform.position, Quaternion.identity);
                Destroy(_base);
            }
            else
            {
                GetComponent<AudioSource>().clip = Error;
                SM.PlaySound(GetComponent<AudioSource>());
            }
        } 
    }

    public void LevelUp()
    {

        if (LM.coins >= _tower.PriceLevelUp)
        {
            GetComponent<AudioSource>().clip = Buy;
            SM.PlaySound(GetComponent<AudioSource>());

            LM.coins -= _tower.PriceLevelUp;
            _tower.LevelUp();
        }
        else
        {
            GetComponent<AudioSource>().clip = Error; 
            SM.PlaySound(GetComponent<AudioSource>());
        }   
    }
}
