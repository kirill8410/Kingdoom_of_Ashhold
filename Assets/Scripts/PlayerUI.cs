using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private LevelManager LM;
    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] TextMeshProUGUI HP;
    [SerializeField] TextMeshProUGUI wave;
    [SerializeField] GameObject but;
    private int wave1;

    private void Start()
    {
        LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    private void Update()
    {
        coins.text = LM.coins.ToString();
        HP.text = LM.HP.ToString();
        wave.text = $"{LM.wave}/{LM.waves.Length}";
        but.SetActive(!LM.isWave);
    }

    public void NormalButtonClick()
    {
        LM.NormalTime();
    }
    public void FastlButtonClick()
    {
        LM.FastTime();
    }
    public void PauselButtonClick()
    {
        LM.Pause();
    }
    public void StartlButtonClick()
    {
        LM.StartWave();
    }
}
