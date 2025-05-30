using NUnit.Framework;
using TMPro;
using Unity.XR.CoreUtils;
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

    #region Разделы

    GameObject _information;
    GameObject _selection;
    GameObject _informationLevelUp;

    #endregion

    #region Тексты

    TextMeshProUGUI _name;
    TextMeshProUGUI _price;
    TextMeshProUGUI _damage;
    TextMeshProUGUI _distance;
    TextMeshProUGUI _attackSpeed;

    TextMeshProUGUI _textLevelUp;
    TextMeshProUGUI _damageLevelUp;
    TextMeshProUGUI _distanceLevelUp;
    TextMeshProUGUI _attackSpeedLevelUp;
    TextMeshProUGUI _priceLevelUp;

    #endregion

    #region Иконки


    Image _imageDamage;
    Image _imageDamageLevelUp;

    #endregion

    [Header("Button")]
    [SerializeField] Button _buildButton;
    [SerializeField] Button[] _evolutionButtons;
    [SerializeField] Button _levelUpButton;

    #region Звуки

    AudioClip _soundBuy;
    AudioClip _soundError;

    #endregion

    
    [SerializeField] GameObject _base_1;

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
        _informationLevelUp.SetActive(false);
        _selection.SetActive(true);

        #endregion

        #region Тексты

        _name = _information.GetNamedChild("Name").GetComponent<TextMeshProUGUI>();

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
            if (_tower.TowerLevel != 3) // Отображение кнопок прокачки если уровень не максимальный
            {
                _levelUpButton.gameObject.SetActive(true);
                if (_evolutionButtons.Length > 0)
                {
                    foreach (Button EvolutionButton in _evolutionButtons)
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
                    foreach (Button EvolutionButton in _evolutionButtons)
                    {
                        EvolutionButton.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void ShowInformation(TowerParameters tower) // Паказываем информацию о башне
    {
        if (_information.activeSelf == false || _selectTower != tower) // Показать информацию если информация не показана или показана о другой башне
        {
            _selectTower = tower;

            _information.SetActive(true);
            _selection.transform.localPosition = new Vector3(-80f, 0f, 0f);

            _name.text = tower.TowerName;
            _price.text = tower.Price_1.ToString();
            _damage.text = tower.Damage_1.ToString();
            _distance.text = ((tower.AttackDistance_1 / 4) - 0.5f).ToString();
            _attackSpeed.text = tower.AttackSpeed_1.ToString();
        }
        else if ((_information.activeSelf == true && _selectTower == tower) || GetComponent<Canvas>().enabled == false) // Скрыть информацию
        {
            _selectTower = null;
            _information.SetActive(false);
            _selection.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }
    public void Build() // Построить башню
    {
        if (_base_1 != null)
        {
            if (LM._coins >= _selectTower.Price_1)
            {
                GetComponent<AudioSource>().clip = _soundBuy;
                SM.PlaySound(GetComponent<AudioSource>());

                LM._coins -= _selectTower.Price_1;
                Instantiate(_selectTower.TowerPrefab, _base_1.transform.position, Quaternion.identity);
                Destroy(_information, 0.1f);
                Destroy(_base_1, 4f);
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
            _selection.transform.localPosition = new Vector3(-80f, 0f, 0f);
            if (_tower.TowerLevel == 1)
            {
                _damageLevelUp.text = $"{_tower.Parameters.Damage_2}\n(+{_tower.Parameters.Damage_2 - _tower.Parameters.Damage_1})";
                _textLevelUp.text = "Улучшить до уроня 2";
                _distanceLevelUp.text = $"{_tower.Parameters.AttackDistance_2 / 4}" +
                    $"\n(+{(_tower.Parameters.AttackDistance_2 - _tower.Parameters.AttackDistance_1) / 4})";
                _attackSpeedLevelUp.text = $"{_tower.Parameters.AttackSpeed_2}" +
                    $"\n(+{_tower.Parameters.AttackSpeed_2 - _tower.Parameters.AttackSpeed_1})";
                _priceLevelUp.text = _tower.Parameters.Price_2.ToString();
            }
            else if (_tower.TowerLevel == 2)
            {
                _damageLevelUp.text = $"{_tower.Parameters.Damage_3}\n(+{_tower.Parameters.Damage_3 - _tower.Parameters.Damage_2})";
                _textLevelUp.text = "Улучшить до уроня 3";
                _distanceLevelUp.text = $"{_tower.Parameters.AttackDistance_3 / 4}" +
                    $"\n(+{(_tower.Parameters.AttackDistance_3 - _tower.Parameters.AttackDistance_2) / 4})";
                _attackSpeedLevelUp.text = $"{_tower.Parameters.AttackSpeed_3}" +
                    $"\n(+{_tower.Parameters.AttackSpeed_3 - _tower.Parameters.AttackSpeed_2})";
                _priceLevelUp.text = _tower.Parameters.Price_3.ToString();
            }

        }
        else if ((_informationLevelUp.activeSelf == true) || GetComponent<Canvas>().enabled == false) // Скрыть информацию
        {
            _informationLevelUp.SetActive(false);
            _selection.transform.localPosition = new Vector3(0f, 0f, 0f);
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
