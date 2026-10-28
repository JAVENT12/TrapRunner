using UnityEngine;

public class BotController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3.25f;
    public float jumpForce = 10f;

    [Header("Goal Failsafe")]
    public float finishFailsafeX = 10f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip jumpSound;

    private Rigidbody2D rb;
    private Vector3 startPosition;

    private bool isGrounded = false;
    private bool finishTriggered = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (GameManager.Instance != null &&
            !GameManager.Instance.IsPlaying())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = new Vector2(
            moveSpeed,
            rb.linearVelocity.y
        );

        // Failsafe if Bot somehow passes the Goal trigger
        if (!finishTriggered &&
            transform.position.x >= finishFailsafeX)
        {
            finishTriggered = true;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.BotReachedFlag(this);
            }
        }
    }

    public void TryJump()
    {
        if (!isGrounded)
            return;

        isGrounded = false;

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            jumpForce
        );

        if (audioSource != null &&
            jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    public void ResetToStart()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = startPosition;

        isGrounded = false;
        finishTriggered = false;
    }

    public void Die()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.BotKilled();
        }

        gameObject.SetActive(false);
    }
}