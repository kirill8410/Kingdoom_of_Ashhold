using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour // Все функции меню
{
    // Окна меню (начальное, настройки и быбор сложности)
    [SerializeField] GameObject StartButtons;
    [SerializeField] GameObject SettingsButtons;
    [SerializeField] GameObject PlayButtons;
    // Переменные нужные для проверки того какое окно сейчас открыто
    private bool isSettings = false;
    private bool isStarting = false;

    private void Start()
    {
        // открываем нужные и закрываем ненужные окна (на всякий случай)
        HideButtons(SettingsButtons);
        HideButtons(PlayButtons);
        ShowButtons(StartButtons);
    }

    public void ExitGame() // Выход из игры
    {
        Application.Quit();
    }
    public void Settings() 
    {
        if (!isSettings) // Открытие окна настроек
        {
            HideButtons(StartButtons);
            ShowButtons(SettingsButtons);
            isSettings = true;
        }
        else // Закрытие окна настроек
        {
            HideButtons(SettingsButtons);
            ShowButtons(StartButtons);
            isSettings = false;
        }
    }
    public void StartGame()
    {
        if (!isStarting) // Открытие выбора сложности
        {
            if (PlayerPrefs.GetInt("Difficulty") > 0) // Запуск игры если сложность выброна
            {
                SceneManager.LoadScene("GameLobby");
            }
            else // Выбор сложности
            {
                HideButtons(StartButtons);
                ShowButtons(PlayButtons);
                isStarting = true;
            }
        }
        else // Закрытие окна выбора сложности
        {
            HideButtons(PlayButtons);
            ShowButtons(StartButtons);
            isStarting = false;
        }
    }
    private void HideButtons(GameObject Buttons) // Открытие окна (нужна только потому что Кирилл изначально хотел
                                                 // показывать несколько объектов, а потом понял что их просто
                                                 // можно засунуть в пустышку, а функцию было лень удолять)
    {
        Buttons.SetActive(false);
    }
    private void ShowButtons(GameObject Buttons) // Закрытие окна (таже причина)
    {
        Buttons.SetActive(true);
    }
}
