using System.Linq;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class GoblinsInformation : MonoBehaviour
{
    public Sprite PhysicalProtection;
    public Sprite MageProtection;

    public Button NextButton;
    public Button BackButton;
    private GoblinInformationButton[] _buttons = new GoblinInformationButton[5];

    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI HP;
    public TextMeshProUGUI Protection;
    public TextMeshProUGUI Speed;
    public TextMeshProUGUI Coins;

    public Image ProtectionImage;

    private int _numberButtons = 0;

    private EnemyParameters[] _enemyParameters;

    private void Start()
    {
        _enemyParameters = Resources.LoadAll<EnemyParameters>("ScriptableObject/Parameters/Enemy");

        NextButton.onClick.AddListener(Next);
        BackButton.onClick.AddListener(Back);

        _buttons = GetComponentsInChildren<GoblinInformationButton>();

        Name.text = "";
        Description.text = "";
        HP.text = "";
        Protection.text = "";
        Speed.text = "";
        Coins.text = "";

        UpdateButtons();
    }

    private void Next()
    {
        if (_numberButtons < _enemyParameters.Length / 5)
        {
            _numberButtons++;
        }
        UpdateButtons();
    }

    private void Back()
    {
        if (_numberButtons > 0)
        {
            _numberButtons--;
        }
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        if (_numberButtons > 0)
        {
            BackButton.gameObject.SetActive(true);
        }
        else
        {
            BackButton.gameObject.SetActive(false);
        }
        if (_numberButtons < _enemyParameters.Length / 5)
        {
            NextButton.gameObject.SetActive(true);
        }
        else
        {
            NextButton.gameObject.SetActive(false);
        }
        for (int i = _numberButtons * 5; i < _numberButtons * 5 + 5; i++)
        {
            _buttons[(i) % 5].gameObject.SetActive(true);
            if (i < _enemyParameters.Length)
            {
                _buttons[(i) % 5].SetEnemyParameters(this, _enemyParameters[i]);
            }
            else
            {
                _buttons[(i) % 5].gameObject.SetActive(false);
            }
        }
    }
}
