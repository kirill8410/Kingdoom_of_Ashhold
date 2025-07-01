using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private InputActionAsset _inputActions;

    #region Actions

    private InputAction _openMenuAction;


    #endregion

    private LevelManager LM;
    private TourneyLevelManager TLM;
    private GameObject _player;

    private TextMeshProUGUI _coinsText;
    private TextMeshProUGUI _HPText;
    private TextMeshProUGUI _waveText;

    private TextMeshProUGUI _finishText;

    private TextMeshProUGUI _waveInformationText;

    #region Кнопки

    private Button _normalTimeButton;
    private Button _fastTimeButton;
    private Button _skipWaveButton;
    private Button _waveInformationButton;
    private Button _startWaveButton;
    private Button _exitButton;

    private Button _finishButton;

    #endregion

    #region Разделы

    private GameObject _sectionMenu;
    private GameObject _sectionFinish;

    private GameObject _subsectionMenu;
    private GameObject _subsectionWaveInformation;
    private GameObject _subsectionQuestion;

    #endregion

    #region Question

    private TextMeshProUGUI _questionText;
    private Button _noButton;
    private Button _yesButton;

    #endregion

    private bool _isFinish = false;

    private void Start()
    {
        _inputActions = Resources.Load<InputActionAsset>("InputActions");
        if (GameObject.Find("LevelManager") != null)
        {
            LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        }
        else
        {
            TLM = GameObject.Find("TourneyLevelManager").GetComponent<TourneyLevelManager>();
        }
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
        _sectionFinish = gameObject.GetNamedChild("Finish");

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
        _waveText = _sectionMenu.GetNamedChild("Wave").GetComponent<TextMeshProUGUI>();
        
        _finishText = _sectionFinish.GetNamedChild("Finish Text").GetComponent<TextMeshProUGUI>();

        _waveInformationText = _subsectionWaveInformation.GetNamedChild("Information Text").GetComponent<TextMeshProUGUI>();

        #region Кнопки

        _normalTimeButton = _sectionMenu.GetNamedChild("Normal Time Button").GetComponent<Button>();
        _fastTimeButton = _sectionMenu.GetNamedChild("Fast Time Button").GetComponent<Button>();
        _skipWaveButton = _sectionMenu.GetNamedChild("Skip Wave Button").GetComponent<Button>();
        _waveInformationButton = _sectionMenu.GetNamedChild("Wave Information Button").GetComponent<Button>();
        _startWaveButton = _sectionMenu.GetNamedChild("Start Wave Button").GetComponent<Button>();
        _exitButton = _sectionMenu.GetNamedChild("Exit Button").GetComponent<Button>();

        _finishButton = _sectionFinish.GetNamedChild("Finish Button").GetComponent<Button>();

        _normalTimeButton.onClick.AddListener(NormalTime);
        _fastTimeButton.onClick.AddListener(FastTime);
        _skipWaveButton.onClick.AddListener(SkipWave);
        _waveInformationButton.onClick.AddListener(InformationWave);
        _startWaveButton.onClick.AddListener(StartWave);
        _exitButton.onClick.AddListener(Exit);
        if (LM != null)
        {
            _finishButton.onClick.AddListener(LM.ReturtToLobby);
        }
        else
        {
            _finishButton.onClick.AddListener(TLM.ReturtToLobby);
        }

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
        if (LM != null)
        {
            _coinsText.text = LM._coins.ToString();
            _HPText.text = LM.GetHP().ToString();
            _waveText.text = $"{LM.GetWave()}/{LM.GetWaves().Length}";
        }
        else
        {
            _coinsText.text = TLM._coins.ToString();
            _HPText.text = TLM.GetHP().ToString();
            _waveText.text = $"{TLM.GetNumberWave()}";
        }

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

        if (_openMenuAction.triggered && !_isFinish)
        {
            if (!_sectionMenu.activeSelf)
            {
                _sectionMenu.SetActive(true);
                _sectionFinish.SetActive(false);
                StabilizeMenu();
            }
            else
            {
                _sectionMenu.SetActive(false);
            }
        }

        #endregion

        #region Кнопки

        if (LM != null)
        {
            if (LM.GetWaveContinues())
            {
                _startWaveButton.enabled = false;
            }
            else
            {
                _startWaveButton.enabled = true;
            }
        }
        else
        {
            if (TLM.GetWaveContinues())
            {
                _startWaveButton.enabled = false;
            }
            else
            {
                _startWaveButton.enabled = true;
            }
        }

        #endregion

    }

    #region Menu 

    private void FastTime()
    {
        Time.timeScale = 2.0f;
    }

    private void NormalTime()
    {
        Time.timeScale = 1.0f;
    }

    private void SkipWave()
    {
        if (LM != null)
        {
            if (LM.GetWaveContinues())
            {
                _subsectionMenu.transform.localPosition = new Vector3(-80, 0, 0);

                _subsectionQuestion.SetActive(true);
                _subsectionQuestion.transform.localPosition = new Vector3(50, 0, 0);

                _questionText.text = "Вы уверены что хотите закончить волну?";
                _yesButton.onClick.AddListener(LM.SkipWave);
            }
        }
        else
        {
            if (TLM.GetWaveContinues())
            {
                _subsectionMenu.transform.localPosition = new Vector3(-80, 0, 0);

                _subsectionQuestion.SetActive(true);
                _subsectionQuestion.transform.localPosition = new Vector3(50, 0, 0);

                _questionText.text = "Вы уверены что хотите закончить волну?";
                _yesButton.onClick.AddListener(TLM.SkipWave);
            }
        }
    }

    private void InformationWave()
    {
        if (LM != null)
        {
            if (!_subsectionWaveInformation.activeSelf)
            {
                if (LM.GetWave() < LM.GetWaves().Length)
                {
                    Wave nextWave = LM.GetWaves()[LM.GetWave()];
                    Dictionary<string, int> enemyes = new Dictionary<string, int>();
                    for (int i = 0; i < nextWave.Enemies.Length; i++)
                    {
                        if (enemyes.ContainsKey(nextWave.Enemies[i].GetComponent<Enemy>().GetName()))
                        {
                            enemyes[nextWave.Enemies[i].GetComponent<Enemy>().GetName()] += nextWave.NumberOfEnemies[i];
                        }
                        else
                        {
                            enemyes.Add(nextWave.Enemies[i].GetComponent<Enemy>().GetName(), nextWave.NumberOfEnemies[i]);
                        }
                    }
                    string text = "";
                    foreach (KeyValuePair<string, int> kvp in enemyes)
                    {
                        text += $"{kvp.Key}: {kvp.Value}\n";
                    }

                    _subsectionMenu.transform.localPosition = new Vector3(80, 0, 0);

                    _subsectionWaveInformation.SetActive(true);
                    _subsectionWaveInformation.transform.localPosition = new Vector3(-50, 0, 0);

                    _waveInformationText.text = text;
                }
            }
            else
            {
                StabilizeMenu();
            }
        }
        else
        {
            if (!_subsectionWaveInformation.activeSelf)
            {
                Wave nextWave = TLM.GetWave();
                Dictionary<string, int> enemyes = new Dictionary<string, int>();
                for (int i = 0; i < nextWave.Enemies.Length; i++)
                {
                    if (nextWave.Enemies[i] != null)
                    {
                        if (enemyes.ContainsKey(nextWave.Enemies[i].GetComponent<Enemy>().GetName()))
                        {
                            enemyes[nextWave.Enemies[i].GetComponent<Enemy>().GetName()] += nextWave.NumberOfEnemies[i];
                        }
                        else
                        {
                            enemyes.Add(nextWave.Enemies[i].GetComponent<Enemy>().GetName(), nextWave.NumberOfEnemies[i]);
                        }
                    }       
                }
                string text = "";
                foreach (KeyValuePair<string, int> kvp in enemyes)
                {
                    text += $"{kvp.Key}: {kvp.Value}\n";
                }

                _subsectionMenu.transform.localPosition = new Vector3(80, 0, 0);

                _subsectionWaveInformation.SetActive(true);
                _subsectionWaveInformation.transform.localPosition = new Vector3(-50, 0, 0);

                _waveInformationText.text = text;
            }
            else
            {
                StabilizeMenu();
            }
        }
    }

    private void StartWave()
    {
        if (LM != null)
        {
            LM.StartWave();
            StabilizeMenu();
        }
        else
        {
            TLM.StartWave();
            StabilizeMenu();
        }
    }

    private void Exit()
    {
        _subsectionMenu.transform.localPosition = new Vector3(-80, 0, 0);

        _subsectionQuestion.SetActive(true);
        _subsectionQuestion.transform.localPosition = new Vector3(50, 0, 0);

        _questionText.text = "Вы уверены что хотите выйти?";
        if (LM != null)
        {
            _yesButton.onClick.AddListener(LM.ReturtToLobby);
        }
        else
        {
            _yesButton.onClick.AddListener(TLM.ReturtToLobby);
        }
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

    public void Lose()
    {
        if (LM != null)
        {
            _sectionMenu.SetActive(false);
            _sectionFinish.SetActive(true);
            _isFinish = true;
            _finishText.text = "Поражение\n:(";
        }
        else
        {
            _sectionMenu.SetActive(false);
            _sectionFinish.SetActive(true);
            _isFinish = true;
            _finishText.text = $"Ваш счет:\n{TLM.Points}";
        }
    }

    public void Win()
    {
        _sectionMenu.SetActive(false);
        _sectionFinish.SetActive(true);
        _isFinish = true;
        _finishText.text = "Победа!";
    }

    #endregion
}
