using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TrapManager : MonoBehaviour
{
    public enum TrapType
    {
        Spike,
        FallingPlatform,
        Spring
    }

    [Header("Trap Prefabs")]
    public GameObject spikeTrapPrefab;
    public GameObject fallingPlatformPrefab;
    public GameObject springTrapPrefab;

    [Header("Trap Inventory")]
    public int trapsRemaining = 3;

    [Header("Falling")]
    public float trapGravityScale = 2.4f;

    [Header("UI")]
    public TMP_Text trapsRemainingText;
    public TMP_Text selectedTrapText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip trapPlaceSound;
    public AudioClip trapDropSound;

    private TrapType selectedTrap =
        TrapType.Spike;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(
                SceneManager.GetActiveScene().name
            );

            return;
        }

        if (GameManager.Instance != null &&
            (!GameManager.Instance.IsPlaying() ||
             GameManager.Instance.IsPaused()))
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedTrap = TrapType.Spike;
            UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedTrap = TrapType.FallingPlatform;
            UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedTrap = TrapType.Spring;
            UpdateUI();
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceTrap();
        }
    }

    void TryPlaceTrap()
    {
        if (trapsRemaining <= 0)
        {
            return;
        }

        Vector3 mousePos =
            Input.mousePosition;

        mousePos.z =
            -Camera.main.transform.position.z;

        Vector2 worldPos =
            Camera.main.ScreenToWorldPoint(
                mousePos
            );

        Collider2D hit =
            Physics2D.OverlapPoint(
                worldPos
            );

        if (hit == null)
        {
            return;
        }

        TrapSlot slot =
            hit.GetComponent<TrapSlot>();

        if (slot == null ||
            slot.occupied)
        {
            return;
        }

        GameObject trapPrefab =
            GetSelectedTrapPrefab();

        if (trapPrefab == null)
        {
            return;
        }

        GameObject newTrap =
            Instantiate(
                trapPrefab,
                slot.transform.position,
                Quaternion.identity
            );

        Rigidbody2D trapRb =
            newTrap.GetComponent<Rigidbody2D>();

        if (trapRb != null)
        {
            trapRb.bodyType =
                RigidbodyType2D.Dynamic;

            trapRb.gravityScale =
                trapGravityScale;

            // Play drop sound
            if (audioSource != null &&
                trapDropSound != null)
            {
                audioSource.PlayOneShot(
                    trapDropSound
                );
            }
        }

        slot.occupied = true;
        trapsRemaining--;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.TrapUsed();
        }

        if (audioSource != null &&
            trapPlaceSound != null)
        {
            audioSource.PlayOneShot(
                trapPlaceSound
            );
        }

        UpdateUI();
    }

    GameObject GetSelectedTrapPrefab()
    {
        switch (selectedTrap)
        {
            case TrapType.Spike:
                return spikeTrapPrefab;

            case TrapType.FallingPlatform:
                return fallingPlatformPrefab;

            case TrapType.Spring:
                return springTrapPrefab;

            default:
                return null;
        }
    }

    void UpdateUI()
    {
        if (trapsRemainingText != null)
        {
            trapsRemainingText.text =
                "Traps Remaining: " +
                trapsRemaining;
        }

        if (selectedTrapText != null)
        {
            selectedTrapText.text =
                "Selected Trap: " +
                selectedTrap;
        }
    }
}