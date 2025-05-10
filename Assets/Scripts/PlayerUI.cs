using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private LevelManager LM;
    private TextMeshProUGUI _coinsText;
    private TextMeshProUGUI _HPText;
    private TextMeshProUGUI _WaveText;

    [SerializeField] Button _btn;

    private void Start()
    {
        LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        _coinsText = gameObject.GetNamedChild("Coins").GetComponent<TextMeshProUGUI>();
        _HPText = gameObject.GetNamedChild("HP").GetComponent<TextMeshProUGUI>();
        _WaveText = gameObject.GetNamedChild("Wave").GetComponent<TextMeshProUGUI>();

        _btn.onClick.AddListener(FastTime);
    }

    private void Update()
    {
        _coinsText.text = LM.coins.ToString();
        _HPText.text = LM.HP.ToString();
        _WaveText.text = $"{LM.wave}/{LM.MaxWave}";
    }

    public void FastTime()
    {
        Time.timeScale = 2.0f;
        print("click");
    }
    public void NormalTime()
    {
        Time.timeScale = 1.0f;
    }
    public void StartlButtonClick()
    {
        LM.StartWave();
    }
}
