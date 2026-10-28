using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Playing,
        Finished
    }

    public static GameManager Instance;

    [Header("Game State")]
    public GameState currentState = GameState.Playing;

    [Header("Scores")]
    public int playerScore = 0;
    public int botScore = 0;

    [Header("Round Tracking")]
    public int trapsUsed = 0;
    public int botFinishes = 0;
    public int maxBotFinishes = 3;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text statusText;
    public TMP_Text resultText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip playerWinSound;
    public AudioClip botFinishSound;

    private Coroutine statusRoutine;
    private bool paused = false;

    void Awake()
    {
        Instance = this;

        Time.timeScale = 1f;

        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true);
        }

        if (statusText != null)
        {
            statusText.gameObject.SetActive(false);
        }

        if (resultText != null)
        {
            resultText.gameObject.SetActive(false);
        }

        UpdateScoreUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (!IsPlaying())
            return;

        paused = !paused;

        Time.timeScale = paused ? 0f : 1f;
    }

    public bool IsPlaying()
    {
        return currentState == GameState.Playing;
    }

    public bool IsPaused()
    {
        return paused;
    }

    public void TrapUsed()
    {
        if (!IsPlaying())
            return;

        trapsUsed++;
        UpdateScoreUI();
    }

    public void BotReachedFlag(BotController bot)
    {
        if (!IsPlaying())
            return;

        botFinishes++;
        botScore += 10;

        UpdateScoreUI();

        if (audioSource != null &&
            botFinishSound != null)
        {
            audioSource.PlayOneShot(botFinishSound);
        }

        if (botFinishes >= maxBotFinishes)
        {
            EndRound();
            return;
        }

        if (statusRoutine != null)
        {
            StopCoroutine(statusRoutine);
        }

        statusRoutine = StartCoroutine(
            ShowTemporaryStatus(
                "BOT FINISH " +
                botFinishes +
                "/3   +10"
            )
        );

        if (bot != null)
        {
            bot.ResetToStart();
        }
    }

    public void BotKilled()
    {
        if (!IsPlaying())
            return;

        switch (trapsUsed)
        {
            case 1:
                playerScore = 30;
                break;

            case 2:
                playerScore = 20;
                break;

            case 3:
                playerScore = 10;
                break;

            default:
                playerScore = 0;
                break;
        }

        UpdateScoreUI();
        EndRound();
    }

    void EndRound()
    {
        if (!IsPlaying())
            return;

        currentState = GameState.Finished;

        paused = false;
        Time.timeScale = 1f;

        if (statusRoutine != null)
        {
            StopCoroutine(statusRoutine);
            statusRoutine = null;
        }

        if (statusText != null)
        {
            statusText.gameObject.SetActive(false);
        }

        string result;

        if (playerScore > botScore)
        {
            result = "PLAYER WINS!";

            if (audioSource != null &&
                playerWinSound != null)
            {
                audioSource.PlayOneShot(playerWinSound);
            }
        }
        else if (botScore > playerScore)
        {
            result = "BOT WINS!";
        }
        else
        {
            result = "TIE!";
        }

        UpdateScoreUI();
        ShowResult(result);
    }

    void UpdateScoreUI()
    {
        if (scoreText == null)
            return;

        scoreText.gameObject.SetActive(true);

        scoreText.text =
            "Player: " +
            playerScore +
            "     Bot: " +
            botScore;
    }

    IEnumerator ShowTemporaryStatus(string message)
    {
        if (statusText == null)
            yield break;

        statusText.text = message;
        statusText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.25f);

        statusText.gameObject.SetActive(false);
        statusRoutine = null;
    }

    void ShowResult(string result)
    {
        if (resultText != null)
        {
            resultText.text =
                result +
                "\nPlayer: " +
                playerScore +
                "     Bot: " +
                botScore +
                "\nPress R to Restart";

            resultText.gameObject.SetActive(true);
        }
    }
}