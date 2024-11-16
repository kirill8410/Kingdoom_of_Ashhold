using UnityEngine;
using UnityEngine.UI;

public class Trigger : MonoBehaviour // Работа Триггера
{
    // Активен ли триггер
    public bool IsActive;
    // Изображения включенного и выключеного триггера
    [SerializeField] Sprite activeIcon;
    [SerializeField] Sprite notActiveIcon;
    // Объекты icon и iconChanged триггера
    [SerializeField] Image icon;
    [SerializeField] Image iconChanged;
    // Информация которую будет хранить триггер (эта информация равна true или false)
    public string saveInfo;
    private void Update()
    {
        // Изменение вешнего вида триггера 
        if (IsActive)
        {
            icon.sprite = activeIcon;
            iconChanged.sprite = notActiveIcon;
        }
        else
        {
            icon.sprite = notActiveIcon;
            iconChanged.sprite = activeIcon;
        }
    }

    public void Press()
    {
        // сохранение информации при включении и выключении триггера
        if (!IsActive)
        {
            PlayerPrefs.SetString(saveInfo, "true");
            IsActive = true;
        }
        else
        {
            PlayerPrefs.SetString(saveInfo, "false");
            IsActive = false;
        }
        PlayerPrefs.Save();
    }
}
