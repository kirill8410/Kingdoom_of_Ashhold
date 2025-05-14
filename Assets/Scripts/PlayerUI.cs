using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private InputActionAsset _inputActions;

    #region Actions

    private InputAction _openMenuAction;


    #endregion

    private LevelManager LM;
    private GameObject _player;

    private TextMeshProUGUI _coinsText;
    private TextMeshProUGUI _HPText;
    private TextMeshProUGUI _WaveText;

    #region Кнопки

    private Button _normalTimeButton;
    private Button _fastTimeButton;
    private Button _skipWaveButton;
    private Button _waveInformationButton;
    private Button _startWaveButton;
    private Button _exitButton;

    #endregion

    #region Разделы

    private GameObject _sectionMenu;
    private GameObject _sectionSpell;

    private GameObject _subsectionMenu;
    private GameObject _subsectionWaveInformation;
    private GameObject _subsectionQuestion;

    #endregion

    #region Question

    private TextMeshProUGUI _questionText;
    private Button _noButton;
    private Button _yesButton;

    #endregion

    private void Start()
    {
        _inputActions = Resources.Load<InputActionAsset>("InputActions");
        LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        _player = FindFirstObjectByType<XROrigin>().gameObject;

        #region Actions

        _openMenuAction = _inputActions.FindAction("Open Menu");

        #endregion

        #region Разделы

        Canvas[] selections = gameObject.GetComponentsInChildren<Canvas>(includeInactive: true);


        foreach (Canvas selection in selections)
        {
            selection.gameObject.SetActive(true);
        }

        _sectionMenu = gameObject.GetNamedChild("Menu");
        _sectionSpell = gameObject.GetNamedChild("Spell");

        _subsectionMenu = _sectionMenu.GetNamedChild("SubMenu");
        _subsectionWaveInformation = _sectionMenu.GetNamedChild("Wave Information");
        _subsectionQuestion = _sectionMenu.GetNamedChild("Question");

        foreach (Canvas selection in selections)
        {
            selection.gameObject.SetActive(false);
        }

        #endregion

        _coinsText = _sectionMenu.GetNamedChild("Coins").GetComponent<TextMeshProUGUI>();
        _HPText = _sectionMenu.GetNamedChild("HP").GetComponent<TextMeshProUGUI>();
        _WaveText = _sectionMenu.GetNamedChild("Wave").GetComponent<TextMeshProUGUI>();

        #region Кнопки

        _normalTimeButton = _sectionMenu.GetNamedChild("Normal Time Button").GetComponent<Button>();
        _fastTimeButton = _sectionMenu.GetNamedChild("Fast Time Button").GetComponent<Button>();
        _skipWaveButton = _sectionMenu.GetNamedChild("Skip Wave Button").GetComponent<Button>();
        _waveInformationButton = _sectionMenu.GetNamedChild("Wave Information Button").GetComponent<Button>();
        _startWaveButton = _sectionMenu.GetNamedChild("Start Wave Button").GetComponent<Button>();
        _exitButton = _sectionMenu.GetNamedChild("Exit Button").GetComponent<Button>();

        _normalTimeButton.onClick.AddListener(NormalTime);
        _fastTimeButton.onClick.AddListener(FastTime);
        _skipWaveButton.onClick.AddListener(FastTime);
        _waveInformationButton.onClick.AddListener(FastTime);
        _startWaveButton.onClick.AddListener(StartWave);
        _exitButton.onClick.AddListener(Exit);

        #endregion

        #region Question

        _questionText = _subsectionQuestion.GetComponentInChildren<TextMeshProUGUI>();
        _yesButton = _subsectionQuestion.GetNamedChild("Yes Button").GetComponent<Button>();
        _noButton = _subsectionQuestion.GetNamedChild("No Button").GetComponent<Button>();

        _noButton.onClick.AddListener(StabilizeMenu);
        #endregion

        StabilizeMenu();
    }

    private void Update()
    {
        /*_coinsText.text = LM.coins.ToString();
        _HPText.text = LM.HP.ToString();
        _WaveText.text = $"{LM.wave}/{LM.MaxWave}";*/

        #region Движение

        if (_player != null)
        {
            if (transform.position != _player.transform.position)
            {
                transform.position = new Vector3(
                    transform.position.x + (_player.transform.position.x - transform.position.x) * Time.deltaTime,
                    transform.position.y + (_player.transform.position.y - transform.position.y) * Time.deltaTime,
                    transform.position.z + (_player.transform.position.z - transform.position.z) * Time.deltaTime);
                if (Vector3.Distance(transform.position, _player.transform.position) <= 0.1f)
                {
                    transform.position = _player.transform.position;
                }
            }
            if (transform.eulerAngles.y != _player.transform.eulerAngles.y)
            {
                transform.eulerAngles = new Vector3(
                    0, transform.eulerAngles.y + (_player.transform.eulerAngles.y - transform.eulerAngles.y) * Time.deltaTime, 0);
                if (Mathf.Abs(transform.eulerAngles.y - _player.transform.eulerAngles.y) <= 2)
                {
                    transform.eulerAngles = _player.transform.eulerAngles;
                }
            }
        }

        #endregion

        #region Управление

        if (_openMenuAction.triggered)
        {
            if (!_sectionMenu.activeSelf)
            {
                _sectionMenu.SetActive(true);
                _sectionSpell.SetActive(false);
                StabilizeMenu();
            }
            else
            {
                _sectionMenu.SetActive(false);
            }
        }

        #endregion
    }

    private void FastTime()
    {
        Time.timeScale = 2.0f;
    }
    private void NormalTime()
    {
        Time.timeScale = 1.0f;
    }
    private void StartWave()
    {
        LM.StartWave();
    }
    private void Exit()
    {
        _subsectionMenu.transform.localPosition = new Vector3(-80, 0, 0);

        _subsectionQuestion.SetActive(true);
        _subsectionQuestion.transform.localPosition = new Vector3(50, 0, 0);

        _questionText.text = "Вы уверены что хотите выйти?";
        _yesButton.onClick.AddListener(LM.ReturtToLobby);
    }

    private void StabilizeMenu()
    {
        _subsectionMenu.SetActive(true);
        _subsectionMenu.transform.localPosition = Vector3.zero;

        _subsectionQuestion.transform.localPosition = new Vector3(130, 0, 0);
        _subsectionQuestion.SetActive(false);

        _subsectionWaveInformation.transform.localPosition = new Vector3(-130, 0, 0);
        _subsectionWaveInformation.SetActive(false);
        
    }
}
