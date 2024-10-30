using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] backgroundSounds;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
