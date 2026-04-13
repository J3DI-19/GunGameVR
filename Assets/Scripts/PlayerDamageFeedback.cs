using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerDamageFeedback : MonoBehaviour
{
    public GameObject damageFlashObject; // assign the RawImage GameObject

    public float flashDuration = 0.15f;

    public AudioSource audioSource;
    public AudioClip damageSound;

    Coroutine flashRoutine;

    void Start()
    {
        if (damageFlashObject != null)
            damageFlashObject.SetActive(false); // ensure off at start
    }

    public void PlayDamageFeedback()
    {
        // 🔊 Sound
        if (audioSource && damageSound)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(damageSound);
        }

        // 🔴 Flash
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        if (damageFlashObject != null)
            damageFlashObject.SetActive(true);

        yield return new WaitForSeconds(flashDuration);

        if (damageFlashObject != null)
            damageFlashObject.SetActive(false);
    }
}