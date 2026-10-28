using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        BotController bot =
            other.GetComponentInParent<BotController>();

        if (bot != null &&
            GameManager.Instance != null &&
            GameManager.Instance.IsPlaying())
        {
            GameManager.Instance.BotReachedFlag(bot);
        }
    }
}