using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    // Менеджеры
    private LevelManager LM;
    private SoundManager SM;

    private TowerFunctions _tower;

    private TowerParameters _selectTower;

    #region Разделы

    GameObject _information;
    GameObject _selection;
    GameObject _informationLevelUp;

    #endregion

    #region Тексты

    TextMeshProUGUI _name;
    TextMeshProUGUI _price;
    TextMeshProUGUI _damage;
    TextMeshProUGUI _attackDistance;
    TextMeshProUGUI _attackSpeed;
    TextMeshProUGUI _breakingProtection;

    TextMeshProUGUI _textLevelUp;
    TextMeshProUGUI _damageLevelUp;
    TextMeshProUGUI _AttackDistanceLevelUp;
    TextMeshProUGUI _attackSpeedLevelUp;
    TextMeshProUGUI _priceLevelUp;
    TextMeshProUGUI _breakingProtectionLevelUp;

    #endregion

    #region Иконки

    Image _imageDamage;
    Image _imageDamageLevelUp;

    Sprite _physicalDamageSprite;
    Sprite _mageDamageSprite;
    Sprite _trueDamageSprite;

    #endregion

    #region Кнопки

    GameObject[] _evolutionButtons;
    Button _levelUpButton;

    #endregion

    #region Звуки

    AudioClip _soundBuy;
    AudioClip _soundError;

    #endregion

    GameObject _distance;
    GameObject _base;

    private void Start() 
    {
        #region Звуки

        _soundBuy = Resources.Load<AudioClip>("Sounds/Buy");
        _soundError = Resources.Load<AudioClip>("Sounds/Error");

        #endregion

        #region Разделы

        RectTransform[] gameObjects = gameObject.GetComponentsInChildren<RectTransform>(true);
        foreach (RectTransform gm in gameObjects)
        {
            gm.gameObject.SetActive(true);
        }

        _information = gameObject.GetNamedChild("Information");
        _selection = gameObject.GetNamedChild("Selection");
        _informationLevelUp = gameObject.GetNamedChild("InformationLevelUp");

        _information.SetActive(false);
        if (_informationLevelUp != null)
        {
            _informationLevelUp.SetActive(false);
        }
        _selection.SetActive(true);

        #endregion

        #region Тексты

        _name = _information.GetNamedChild("Name").GetComponent<TextMeshProUGUI>();
        _price = _information.GetNamedChild("PriceText").GetComponent<TextMeshProUGUI>();
        _damage = _information.GetNamedChild("DamageText").GetComponent<TextMeshProUGUI>();
        _attackSpeed = _information.GetNamedChild("AttackSpeedText").GetComponent<TextMeshProUGUI>();
        _attackDistance = _information.GetNamedChild("AttackDistanceText").GetComponent<TextMeshProUGUI>();
        _breakingProtection = _information.GetNamedChild("BreakingProtectionText").GetComponent<TextMeshProUGUI>();

        _textLevelUp = _informationLevelUp.GetNamedChild("Text").GetComponent<TextMeshProUGUI>();
        _priceLevelUp = _informationLevelUp.GetNamedChild("PriceText").GetComponent<TextMeshProUGUI>();
        _damageLevelUp = _informationLevelUp.GetNamedChild("DamageText").GetComponent<TextMeshProUGUI>();
        _attackSpeedLevelUp = _informationLevelUp.GetNamedChild("AttackSpeedText").GetComponent<TextMeshProUGUI>();
        _AttackDistanceLevelUp = _informationLevelUp.GetNamedChild("AttackDistanceText").GetComponent<TextMeshProUGUI>();
        _breakingProtectionLevelUp = _informationLevelUp.GetNamedChild("BreakingProtectionText").GetComponent<TextMeshProUGUI>();

        #endregion

        #region Иконки

        _imageDamage = _information.GetNamedChild("Damage").GetComponent<Image>();
        _imageDamageLevelUp = _informationLevelUp.GetNamedChild("Damage").GetComponent<Image>();

        _physicalDamageSprite = Resources.LoadAll<Sprite>("Sprites/Tower/BaseIcon")[1];
        _mageDamageSprite = Resources.LoadAll<Sprite>("Sprites/Tower/BaseIcon")[0];

        #endregion

        #region Нахождение менеджеров

        if (GameObject.Find("LevelManager") != null)
        {
            LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        }
        if (GameObject.Find("SoundManager"))
        {
            SM = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        }

        #endregion

        #region Кнопки

        _levelUpButton = _selection.GetNamedChild("Level_Up").GetComponent<Button>();

        RectTransform[] children = _selection.GetComponentsInChildren<RectTransform>();
        List<GameObject> evolutionButtons = new List<GameObject>();
        foreach (RectTransform child in children)
        {
            if (child.gameObject.tag == "EvolutionButton")
            {
                evolutionButtons.Add(child.gameObject);
            }
        }
        _evolutionButtons = evolutionButtons.ToArray<GameObject>();

        GetComponent<BuildButtonsAnimation>().SetIcons(_evolutionButtons);

        #endregion

        _tower = GetComponentInParent<TowerFunctions>();
        _distance = GetComponentInChildren<ParticleSystem>(true).gameObject;

        foreach (Transform b in GetComponentsInParent<Transform>())
        {
            if (b.tag == "Tower")
            {
                _base = b.gameObject;
                break;
            }
        }
    }

    private void Update() 
    {
        if (_tower != null)
        {
            if (Vector3.Distance(GameObject.Find("Player").transform.position, gameObject.transform.position) <= 6.5f)
            {
                GetComponent<Canvas>().enabled = true;
                _distance.gameObject.SetActive(true);
            }
            if (Vector3.Distance(GameObject.Find("Player").transform.position, gameObject.transform.position) > 6.5f)
            {
                _selectTower = null;
                _information.SetActive(false);
                _selection.transform.localPosition = new Vector3(0f, 0f, 0f);
                _informationLevelUp.SetActive(false);
                _selection.transform.localPosition = new Vector3(0f, 0f, 0f);
                _distance.gameObject.SetActive(false);
            }
            if (_tower.TowerLevel != 3 && _tower != null) // Отображение кнопок прокачки если уровень не максимальный
            {
                _levelUpButton.gameObject.SetActive(true);
                if (_evolutionButtons.Length > 0)
                {
                    foreach (GameObject EvolutionButton in _evolutionButtons)
                    {
                        EvolutionButton.gameObject.SetActive(false);
                    }
                }  
            }
            else // Отображение кнопок эволюции если уровень максимальный
            {
                _levelUpButton.gameObject.SetActive(false);
                if (_evolutionButtons.Length > 0)
                {
                    foreach (GameObject EvolutionButton in _evolutionButtons)
                    {
                        EvolutionButton.gameObject.SetActive(true);
                    }
                }
            }
        }
        else
        {
            _distance.gameObject.SetActive(false);
            _levelUpButton.gameObject.SetActive(false);
        }
    }

    public void ShowInformation(TowerParameters tower) // Паказываем информацию о башне
    {
        if (_information.activeSelf == false || _selectTower != tower) // Показать информацию если информация не показана или показана о другой башне
        {
            _selectTower = tower;

            _information.SetActive(true);
            _selection.transform.localPosition = new Vector3(-80f, 135f, 0f);

            _name.text = tower.TowerName;
            _price.text = tower.Price_1.ToString();
            _damage.text = tower.Damage_1.ToString();
            _breakingProtection.text = $"{tower.BreakingProtection_1 * 100}%";
            if (tower.DamageType == Tower.DamageTypes.Physical)
            {
                _imageDamage.sprite = _physicalDamageSprite;
            }
            else if (tower.DamageType == Tower.DamageTypes.Magic)
            {
                _imageDamage.sprite = _mageDamageSprite;
            }
            else if (tower.DamageType == Tower.DamageTypes.True)
            {
                _imageDamage.sprite = _trueDamageSprite;
            }
            _attackDistance.text = ((tower.AttackDistance_1 / 4) - 0.5f).ToString();
            _attackSpeed.text = tower.AttackSpeed_1.ToString();
        }
        else if ((_information.activeSelf == true && _selectTower == tower) || GetComponent<Canvas>().enabled == false) // Скрыть информацию
        {
            _selectTower = null;
            _information.SetActive(false);
            _selection.transform.localPosition = new Vector3(0f, 135f, 0f);
        }
    }
    public void Build() // Построить башню
    {
        if (_base != null)
        {
            if (LM._coins >= _selectTower.Price_1)
            {
                GetComponent<AudioSource>().clip = _soundBuy;
                SM.PlaySound(GetComponent<AudioSource>());

                LM._coins -= _selectTower.Price_1;
                Instantiate(_selectTower.TowerPrefab, _base.transform.position, Quaternion.identity);
                Destroy(_information, 0.1f);
                Destroy(_base, 4f);
            }
            else
            {
                GetComponent<AudioSource>().clip = _soundError;
                SM.PlaySound(GetComponent<AudioSource>());
            }
        }
    }
    public void ShowLevelUpInformation() // Показать информацию о улучшении башни
    {
        if (_informationLevelUp.activeSelf == false) // Показать информацию если информация не показана
        {
            _informationLevelUp.SetActive(true);
            _selection.transform.localPosition = new Vector3(-80f, 135f, 0f);
            if (_tower.TowerLevel == 1)
            {
                _damageLevelUp.text = $"{_tower.Parameters.Damage_2}\n(+{_tower.Parameters.Damage_2 - _tower.Parameters.Damage_1})";
                _textLevelUp.text = "Улучшить до уроня 2";
                _AttackDistanceLevelUp.text = $"{_tower.Parameters.AttackDistance_2 / 4}" +
                    $"\n(+{(_tower.Parameters.AttackDistance_2 - _tower.Parameters.AttackDistance_1) / 4})";
                _attackSpeedLevelUp.text = $"{_tower.Parameters.AttackSpeed_2}" +
                    $"\n(+{_tower.Parameters.AttackSpeed_2 - _tower.Parameters.AttackSpeed_1})";
                _breakingProtectionLevelUp.text = $"{_tower.Parameters.BreakingProtection_2 * 100}%\n" +
                    $"(+{_tower.Parameters.BreakingProtection_2 * 100 - _tower.Parameters.BreakingProtection_1 * 100}%)";
                _priceLevelUp.text = _tower.Parameters.Price_2.ToString();
            }
            else if (_tower.TowerLevel == 2)
            {
                _damageLevelUp.text = $"{_tower.Parameters.Damage_3}\n(+{_tower.Parameters.Damage_3 - _tower.Parameters.Damage_2})";
                _textLevelUp.text = "Улучшить до уроня 3";
                _AttackDistanceLevelUp.text = $"{_tower.Parameters.AttackDistance_3 / 4}" +
                    $"\n(+{(_tower.Parameters.AttackDistance_3 - _tower.Parameters.AttackDistance_2) / 4})";
                _attackSpeedLevelUp.text = $"{_tower.Parameters.AttackSpeed_3}" +
                    $"\n(+{_tower.Parameters.AttackSpeed_3 - _tower.Parameters.AttackSpeed_2})";
                _breakingProtectionLevelUp.text = $"{_tower.Parameters.BreakingProtection_3 * 100}%\n" +
                    $"(+{_tower.Parameters.BreakingProtection_3 * 100 - _tower.Parameters.BreakingProtection_2 * 100}%)";
                _priceLevelUp.text = _tower.Parameters.Price_3.ToString();
            }

        }
        else if ((_informationLevelUp.activeSelf == true) || GetComponent<Canvas>().enabled == false) // Скрыть информацию
        {
            _informationLevelUp.SetActive(false);
            _selection.transform.localPosition = new Vector3(0f, 135f, 0f);
        }
    }

    public void LevelUp() // Повышение уровня башни
    {
        if (_tower.TowerLevel == 1)
        {
            if (LM._coins >= _tower.Parameters.Price_2)
            {
                GetComponent<AudioSource>().clip = _soundBuy;
                SM.PlaySound(GetComponent<AudioSource>());

                LM._coins -= _tower.Parameters.Price_2;
                _tower.LevelUp();

                _informationLevelUp.SetActive(false);
                _selection.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
            else
            {
                GetComponent<AudioSource>().clip = _soundError;
                SM.PlaySound(GetComponent<AudioSource>());
            }
        }
        else if (_tower.TowerLevel == 2)
        {
            if (LM._coins >= _tower.Parameters.Price_2)
            {
                GetComponent<AudioSource>().clip = _soundBuy;
                SM.PlaySound(GetComponent<AudioSource>());

                LM._coins -= _tower.Parameters.Price_2;
                _tower.LevelUp();

                _informationLevelUp.SetActive(false);
                _selection.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
            else
            {
                GetComponent<AudioSource>().clip = _soundError;
                SM.PlaySound(GetComponent<AudioSource>());
            }
        }
    }
}
