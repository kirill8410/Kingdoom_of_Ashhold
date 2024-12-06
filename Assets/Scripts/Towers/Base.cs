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
    [SerializeField] GameObject InformationLevelUp;

    [Header("Texts")]
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI Description;
    [SerializeField] TextMeshProUGUI Price;
    [SerializeField] TextMeshProUGUI Damage;
    [SerializeField] TextMeshProUGUI Distance;
    [SerializeField] TextMeshProUGUI AttackSpeed;
    [SerializeField] TextMeshProUGUI TextLeveUp;
    [SerializeField] TextMeshProUGUI DamageLeveUp;
    [SerializeField] TextMeshProUGUI DistanceLeveUp;
    [SerializeField] TextMeshProUGUI PriceLeveUp;

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
            if (_tower.Towerlevel != 3) // Отображение кнопок прокачки если уровень не максимальный
            {
                LevelUpButton.gameObject.SetActive(true);
                foreach (Button EvolutionButton in EvolutionButtons)
                {
                    EvolutionButton.gameObject.SetActive(false);
                }
            }
            else // Отображение кнопок эволюции если уровень максимальный
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
            Distance.text = ((tower.tower.GetComponent<Tower>().attackDistance / 4) - 0.5f).ToString();
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
        else if ((Information.activeSelf == true && selectTower == tower) || GetComponent<Canvas>().enabled == false) // Скрыть информацию
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
    public void ShowLevelUpInformation() // Показать информацию о улучшении башни
    {
        if (InformationLevelUp.activeSelf == false) // Показать информацию если информация не показана
        {
            InformationLevelUp.SetActive(true);
            Selection.transform.localPosition = new Vector3(-80f, 0f, 0f);
            if (_tower.Towerlevel == 1)
            {
                DamageLeveUp.text = _tower.levelUp.damage_1.ToString();
                TextLeveUp.text = "Улучшить до уроня 2";
                DistanceLeveUp.text = _tower.levelUp.distance_1.ToString();
                PriceLeveUp.text = _tower.levelUp.priceLevelUp_1.ToString();
            }
            else if (_tower.Towerlevel == 2)
            {
                DamageLeveUp.text = _tower.levelUp.damage_2.ToString();
                TextLeveUp.text = "Улучшить до уроня 3";
                DistanceLeveUp.text = _tower.levelUp.distance_2.ToString();
                PriceLeveUp.text = _tower.levelUp.priceLevelUp_2.ToString();
            }

        }
        else if ((InformationLevelUp.activeSelf == true) || GetComponent<Canvas>().enabled == false) // Скрыть информацию
        {
            InformationLevelUp.SetActive(false);
            Selection.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }

    public void LevelUp() // Повышение уровня башни
    {

        if (LM.coins >= _tower.PriceLevelUp)
        {
            GetComponent<AudioSource>().clip = Buy;
            SM.PlaySound(GetComponent<AudioSource>());

            LM.coins -= _tower.PriceLevelUp;
            _tower.LevelUp();

            ShowLevelUpInformation();
        }
        else
        {
            GetComponent<AudioSource>().clip = Error; 
            SM.PlaySound(GetComponent<AudioSource>());
        }   
    }

}
