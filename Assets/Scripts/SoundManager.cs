using UnityEngine;

public class SoundManager : MonoBehaviour // Отвечает за все звуки в игре
{
    private AudioSource audioSource;
    private void Start()
    {
        // Находим AudioSource
        audioSource = GetComponent<AudioSource>();
        if (!bool.Parse(PlayerPrefs.GetString("Music"))) // Выключаем музыку если она выключена в настройках
        {
            audioSource.Stop();
        }
    }
    public void SoundUpdate() // Включаем или выключаем музыку в зависимости от настроек
    {
        if (!bool.Parse(PlayerPrefs.GetString("Music")))
        {
            audioSource.Stop();
        }
        else
        {
            audioSource.Play();
        }
    }
    public void PlaySound(AudioSource Audio) // проигрываем звук если он включён в настройках
    {
        if (bool.Parse(PlayerPrefs.GetString("Music")))
        {
            Audio.Play();
        }
    }
}
