using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip fallSound;

    private bool soundPlayed = false;

    void Start()
    {
        if (!soundPlayed &&
            audioSource != null &&
            fallSound != null)
        {
            audioSource.PlayOneShot(fallSound);
            soundPlayed = true;
        }
    }
}