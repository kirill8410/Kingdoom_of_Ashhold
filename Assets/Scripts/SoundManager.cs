using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] backgroundSounds;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!bool.Parse(PlayerPrefs.GetString("Music")))
        {
            audioSource.playOnAwake = false;
            audioSource.Stop();
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
