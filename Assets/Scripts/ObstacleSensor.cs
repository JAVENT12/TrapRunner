using UnityEngine;

public class ObstacleSensor : MonoBehaviour
{
    private BotController bot;

    void Awake()
    {
        bot = GetComponentInParent<BotController>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            bot.TryJump();
        }
    }
}