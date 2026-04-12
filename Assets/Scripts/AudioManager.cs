using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;
    public AudioClip clickSound;

    void Awake()
    {
        instance = this;
    }

    public void PlayClick()
    {
        audioSource.PlayOneShot(clickSound, 1.5f);
    }
}