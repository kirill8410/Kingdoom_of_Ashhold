using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour // Все функции меню
{
    // Окна меню (начальное, настройки и быбор сложности)
    private GameObject StartButtons;
    private GameObject SettingsButtons;
    private GameObject PlayButtons;
    private GameObject TourneyButtons;
    // Переменные нужные для проверки того какое окно сейчас открыто
    private bool isSettings = false;
    private bool isStarting = false;
    private bool isTourney = false;

    private void Start()
    {
        // Получаем окна
        StartButtons = gameObject.GetNamedChild("StartButtons");
        SettingsButtons = gameObject.GetNamedChild("SettingsButtons");
        PlayButtons = gameObject.GetNamedChild("PlayButtons");
        TourneyButtons = gameObject.GetNamedChild("TourneyButtons");
        // Сброс позиций окон
        StartButtons.transform.localPosition = new Vector3(0, 0, 0);
        SettingsButtons.transform.localPosition = new Vector3(0, 0, 0);
        PlayButtons.transform.localPosition = new Vector3(0, 0, 0);
        TourneyButtons.transform.localPosition = new Vector3(0, 0, 0);
        // открываем нужные и закрываем ненужные окна (на всякий случай)
        HideButtons(SettingsButtons);
        HideButtons(PlayButtons);
        HideButtons(TourneyButtons);
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
            HideButtons(TourneyButtons);
            HideButtons(PlayButtons);
            ShowButtons(SettingsButtons);
            isSettings = true;
        }
        else // Закрытие окна настроек
        {
            HideButtons(SettingsButtons);
            HideButtons(TourneyButtons);
            HideButtons(PlayButtons);
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
                HideButtons(SettingsButtons);
                HideButtons(TourneyButtons);
                ShowButtons(PlayButtons);
                isStarting = true;
            }
        }
        else // Закрытие окна выбора сложности
        {
            HideButtons(PlayButtons);
            HideButtons(SettingsButtons);
            HideButtons(TourneyButtons);
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

    /// <summary>
    /// Открытие окна турнира
    /// </summary>
    public void Tourney()
    {
        if (!isTourney) // Открытие окна турнира
        {
            ShowButtons(TourneyButtons);
            HideButtons(StartButtons);
            HideButtons(SettingsButtons);
            HideButtons(PlayButtons);
            isTourney = true;
        }
        else // Закрытие окна турнира
        {
            ShowButtons(StartButtons);
            HideButtons(PlayButtons);
            HideButtons(SettingsButtons);
            HideButtons(TourneyButtons);
            isTourney = false;
        }
    }
    public void TourneyLobby()
    {
        SceneManager.LoadScene("TourneyLobby");
    }
}
