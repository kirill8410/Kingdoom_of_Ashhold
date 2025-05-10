using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
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

    private void Start()
    {
        LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        _player = FindFirstObjectByType<XROrigin>().gameObject;

        _coinsText = gameObject.GetNamedChild("Coins").GetComponent<TextMeshProUGUI>();
        _HPText = gameObject.GetNamedChild("HP").GetComponent<TextMeshProUGUI>();
        _WaveText = gameObject.GetNamedChild("Wave").GetComponent<TextMeshProUGUI>();

        #region Кнопки

        _normalTimeButton = gameObject.GetNamedChild("Normal Time Button").GetComponent<Button>();
        _fastTimeButton = gameObject.GetNamedChild("Fast Time Button").GetComponent<Button>();
        _skipWaveButton = gameObject.GetNamedChild("Skip Wave Button").GetComponent<Button>();
        _waveInformationButton = gameObject.GetNamedChild("Wave Information Button").GetComponent<Button>();
        _startWaveButton = gameObject.GetNamedChild("Start Wave Button").GetComponent<Button>();
        _exitButton = gameObject.GetNamedChild("Exit Button").GetComponent<Button>();

        _normalTimeButton.onClick.AddListener(NormalTime);
        _fastTimeButton.onClick.AddListener(FastTime);
        _skipWaveButton.onClick.AddListener(FastTime);
        _waveInformationButton.onClick.AddListener(FastTime);
        _startWaveButton.onClick.AddListener(StartWave);
        _exitButton.onClick.AddListener(FastTime);

        #endregion
    }

    private void Update()
    {
        _coinsText.text = LM.coins.ToString();
        _HPText.text = LM.HP.ToString();
        _WaveText.text = $"{LM.wave}/{LM.MaxWave}";

        #region Движение

        if (transform.position != _player.transform.position)
        {
            transform.position = new Vector3(
                transform.position.x + (_player.transform.position.x - transform.position.x) * Time.deltaTime, 
                transform.position.y + (_player.transform.position.y - transform.position.y) * Time.deltaTime, 
                transform.position.z + (_player.transform.position.z - transform.position.z) * Time.deltaTime);
            if (Vector3.Distance(transform.position, _player.transform.position) <= 0.1f)
            {
                transform .position = _player.transform.position;
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
}
