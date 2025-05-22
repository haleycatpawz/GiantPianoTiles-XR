using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public int gameScore = 0;
    [SerializeField] TMPro.TextMeshPro scoreText;

    [SerializeField] int numberOfStrikes = 0;

    public bool GameHasStarted;
    public bool gameIsPlaying;

    [SerializeField] UnityEvent gameStarted;
    [SerializeField] UnityEvent gameEnded;
    [SerializeField] UnityEvent gameLose;
    [SerializeField] UnityEvent gameWin;

   // [SerializeField] 

    public void StartGame()
    {
        gameScore = 0;
        numberOfStrikes = 0;
        scoreText.text = gameScore.ToString();

        gameIsPlaying = true;
        GameHasStarted = true;

    }
    private void ResetGame()
    {
        gameScore = 0;
        numberOfStrikes = 0;
        scoreText.text = gameScore.ToString();

        gameIsPlaying = false;
        GameHasStarted = false;

    }
    public void IncreaseGameScore()
    {
        if (gameIsPlaying)
        {
            gameScore += 1;
            scoreText.text = gameScore.ToString();
        }
    }

    public void addStrike()
    {
        if (gameIsPlaying)
        {
            numberOfStrikes += 1;

        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        ResetGame();
    }




    void Update()
    {

    }

    private Coroutine timeCoroutine;

    public void SlowDownGame()
    {
        if (gameIsPlaying == true) // toggle slow-mo
        {
            if (timeCoroutine != null)
                StopCoroutine(timeCoroutine);

            if (Mathf.Approximately(Time.timeScale, 1f))
                timeCoroutine = StartCoroutine(ChangeTimeScale(1f, 0.001f, 1f)); // Slow time over 1 second
            else
                timeCoroutine = StartCoroutine(ChangeTimeScale(Time.timeScale, 1f, 1f)); // Resume time over 1 second
        }
    }

    IEnumerator ChangeTimeScale(float start, float end, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Time.timeScale = Mathf.Lerp(start, end, elapsed / duration);
            elapsed += Time.unscaledDeltaTime; // Use unscaledDeltaTime to remain consistent even when time is slowed
            yield return null;
        }

        Time.timeScale = end;
        Debug.Log($"Time scale set to {end}.");
    }




}
