using UnityEngine;

public class SpringTrap : MonoBehaviour
{
    [Header("Spring")]
    public float launchForce = 12f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip springSound;

    private bool used = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (used)
            return;

        BotController bot =
            collision.gameObject.GetComponentInParent<BotController>();

        if (bot == null)
            return;

        Rigidbody2D botRb =
            bot.GetComponent<Rigidbody2D>();

        if (botRb == null)
            return;

        used = true;

        botRb.AddForce(
            Vector2.up * launchForce,
            ForceMode2D.Impulse
        );

        if (audioSource != null &&
            springSound != null)
        {
            audioSource.PlayOneShot(springSound);
        }
    }
}