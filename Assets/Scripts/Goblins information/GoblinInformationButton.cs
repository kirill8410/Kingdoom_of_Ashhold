using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoblinInformationButton : MonoBehaviour
{
    private EnemyParameters _enemyParameters;

    private Button _button;
    private TextMeshProUGUI _text;

    private GoblinsInformation _goblinsInformation;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SetDescription);

        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetEnemyParameters(GoblinsInformation gb, EnemyParameters parameters)
    {
        _enemyParameters = parameters;
        _goblinsInformation = gb;

        _text.text = _enemyParameters.Name;
    }

    private void SetDescription()
    {
        _goblinsInformation.Description.text = _enemyParameters.Description;
        _goblinsInformation.Name.text = _enemyParameters.Name;
        _goblinsInformation.HP.text = _enemyParameters.MaxHP.ToString();
        _goblinsInformation.Protection.text = _enemyParameters.Protection.ToString();
        _goblinsInformation.Speed.text = _enemyParameters.Speed.ToString();
        _goblinsInformation.Coins.text = _enemyParameters.DropCoins.ToString();

        if (_enemyParameters.ProtectionType == Tower.DamageTypes.Magic)
        {
            _goblinsInformation.ProtectionImage.sprite = _goblinsInformation.MageProtection;
        }
        else if (_enemyParameters.ProtectionType == Tower.DamageTypes.Physical)
        {
            _goblinsInformation.ProtectionImage.sprite = _goblinsInformation.PhysicalProtection;
        }
    }
}
