using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    // Менеджеры
    private LevelManager LM;
    private SoundManager SM;

    private TowerFunctions _tower;

    private TowerParameters _selectTower;

    [Header("Windows")]
    [SerializeField] GameObject Information;
    [SerializeField] GameObject Selection;
    [SerializeField] GameObject InformationLevelUp;
    [SerializeField] GameObject Base_;

    [Header("Texts")]
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI Description;
    [SerializeField] TextMeshProUGUI Price;
    [SerializeField] TextMeshProUGUI Damage;
    [SerializeField] TextMeshProUGUI Distance;
    [SerializeField] TextMeshProUGUI AttackSpeed;
    [SerializeField] TextMeshProUGUI TextLevelUp;
    [SerializeField] TextMeshProUGUI DamageLevelUp;
    [SerializeField] TextMeshProUGUI DistanceLevelUp;
    [SerializeField] TextMeshProUGUI AttackSpeedLevelUp;
    [SerializeField] TextMeshProUGUI PriceLevelUp;

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
        if (GameObject.Find("LevelManager") != null)
        {
            LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        }
        if (GameObject.Find("SoundManager"))
        {
            SM = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        }

        // Нахождение башни на которой находится этот UI
        _tower = GetComponentInParent<TowerFunctions>();
    }

    private void Update() 
    {
        if (_tower != null)
        {
            if (Vector3.Distance(GameObject.Find("Player").transform.position, gameObject.transform.position) <= 6.5f)
            {
                GetComponent<Canvas>().enabled = true;
                Distance.gameObject.SetActive(true);
            }
            if (Vector3.Distance(GameObject.Find("Player").transform.position, gameObject.transform.position) > 6.5f)
            {
                _selectTower = null;
                Information.SetActive(false);
                Selection.transform.localPosition = new Vector3(0f, 0f, 0f);
                InformationLevelUp.SetActive(false);
                Selection.transform.localPosition = new Vector3(0f, 0f, 0f);
                Distance.gameObject.SetActive(false);
            }
            if (_tower.TowerLevel != 3) // Отображение кнопок прокачки если уровень не максимальный
            {
                LevelUpButton.gameObject.SetActive(true);
                if (EvolutionButtons.Length > 0)
                {
                    foreach (Button EvolutionButton in EvolutionButtons)
                    {
                        EvolutionButton.gameObject.SetActive(false);
                    }
                }  
            }
            else // Отображение кнопок эволюции если уровень максимальный
            {
                LevelUpButton.gameObject.SetActive(false);
                if (EvolutionButtons.Length > 0)
                {
                    foreach (Button EvolutionButton in EvolutionButtons)
                    {
                        EvolutionButton.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void ShowInformation(TowerParameters tower) // Паказываем информацию о башне
    {
        if (Information.activeSelf == false || _selectTower != tower) // Показать информацию если информация не показана или показана о другой башне
        {
            _selectTower = tower;

            Information.SetActive(true);
            Selection.transform.localPosition = new Vector3(-80f, 0f, 0f);

            Description.text = tower.TowerDescription;
            Name.text = tower.TowerName;
            Price.text = tower.Price_1.ToString();
            Damage.text = tower.Damage_1.ToString();
            Distance.text = ((tower.AttackDistance_1 / 4) - 0.5f).ToString();
            AttackSpeed.text = tower.AttackSpeed_1.ToString();
        }
        else if ((Information.activeSelf == true && _selectTower == tower) || GetComponent<Canvas>().enabled == false) // Скрыть информацию
        {
            _selectTower = null;
            Information.SetActive(false);
            Selection.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }
    public void Build() // Построить башню
    {
        if (_base != null)
        {
            if (LM._coins >= _selectTower.Price_1)
            {
                GetComponent<AudioSource>().clip = Buy;
                SM.PlaySound(GetComponent<AudioSource>());

                LM._coins -= _selectTower.Price_1;
                Instantiate(_selectTower.TowerPrefab, _base.transform.position, Quaternion.identity);
                Destroy(Information, 0.1f);
                Destroy(_base, 4f);
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
            if (_tower.TowerLevel == 1)
            {
                DamageLevelUp.text = $"{_tower.Parameters.Damage_2}\n(+{_tower.Parameters.Damage_2 - _tower.Parameters.Damage_1})";
                TextLevelUp.text = "Улучшить до уроня 2";
                DistanceLevelUp.text = $"{_tower.Parameters.AttackDistance_2 / 4}" +
                    $"\n(+{(_tower.Parameters.AttackDistance_2 - _tower.Parameters.AttackDistance_1) / 4})";
                AttackSpeedLevelUp.text = $"{_tower.Parameters.AttackSpeed_2}" +
                    $"\n(+{_tower.Parameters.AttackSpeed_2 - _tower.Parameters.AttackSpeed_1})";
                PriceLevelUp.text = _tower.Parameters.Price_2.ToString();
            }
            else if (_tower.TowerLevel == 2)
            {
                DamageLevelUp.text = $"{_tower.Parameters.Damage_3}\n(+{_tower.Parameters.Damage_3 - _tower.Parameters.Damage_2})";
                TextLevelUp.text = "Улучшить до уроня 3";
                DistanceLevelUp.text = $"{_tower.Parameters.AttackDistance_3 / 4}" +
                    $"\n(+{(_tower.Parameters.AttackDistance_3 - _tower.Parameters.AttackDistance_2) / 4})";
                AttackSpeedLevelUp.text = $"{_tower.Parameters.AttackSpeed_3}" +
                    $"\n(+{_tower.Parameters.AttackSpeed_3 - _tower.Parameters.AttackSpeed_2})";
                PriceLevelUp.text = _tower.Parameters.Price_3.ToString();
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
        if (_tower.TowerLevel == 1)
        {
            if (LM._coins >= _tower.Parameters.Price_2)
            {
                GetComponent<AudioSource>().clip = Buy;
                SM.PlaySound(GetComponent<AudioSource>());

                LM._coins -= _tower.Parameters.Price_2;
                _tower.LevelUp();

                InformationLevelUp.SetActive(false);
                Selection.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
            else
            {
                GetComponent<AudioSource>().clip = Error;
                SM.PlaySound(GetComponent<AudioSource>());
            }
        }
        else if (_tower.TowerLevel == 2)
        {
            if (LM._coins >= _tower.Parameters.Price_2)
            {
                GetComponent<AudioSource>().clip = Buy;
                SM.PlaySound(GetComponent<AudioSource>());

                LM._coins -= _tower.Parameters.Price_2;
                _tower.LevelUp();

                InformationLevelUp.SetActive(false);
                Selection.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
            else
            {
                GetComponent<AudioSource>().clip = Error;
                SM.PlaySound(GetComponent<AudioSource>());
            }
        }
    }

    public void Destroy()
    {
        Instantiate(Base_, _base.transform.position, Quaternion.identity);
        Destroy(_base, 0.8f);
    }

}
