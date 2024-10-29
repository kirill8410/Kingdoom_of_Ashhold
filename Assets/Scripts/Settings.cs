using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour // Сохранение и сброс настроек игры (а также сброс всего прогресса)
{
    // Все объекты настроек
    [SerializeField] Trigger Training;
    [SerializeField] Trigger Music;

    private void Start()
    {
        // Задаем объектам настроек сохранённые параметры
        Training.IsActive = bool.Parse(PlayerPrefs.GetString(Training.saveInfo));
        Music.IsActive = bool.Parse(PlayerPrefs.GetString(Music.saveInfo));
    }

    public void ResetProgress() // Сбрасываем прогресс игры
    {
        PlayerPrefs.SetString(Training.saveInfo, "true");
        PlayerPrefs.SetString(Music.saveInfo, "true");
        PlayerPrefs.SetString("Difficulty", "");
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Menu");
    }
}
