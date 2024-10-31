using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!bool.Parse(PlayerPrefs.GetString("Music")))
        {
            audioSource.Stop();
        }
    }
    public void SoundUpdate()
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
    public void PlaySound(AudioSource Audio)
    {
        if (bool.Parse(PlayerPrefs.GetString("Music")))
        {
            Audio.Play();
        }
    }
}
