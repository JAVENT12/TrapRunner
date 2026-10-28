using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        BotController bot =
            collision.gameObject.GetComponentInParent<BotController>();

        if (bot != null)
        {
            bot.Die();
        }
    }
}