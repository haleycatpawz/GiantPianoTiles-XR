using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int gameScore = 0;
    [SerializeField] TMPro.TextMeshPro scoreText;
    [SerializeField] TMPro.TextMeshProUGUI finalScoreText;
  //  [SerializeField] TMPro.TextMeshPro finalScoreText;

    [SerializeField] int numberOfStrikes = 0;

    [SerializeField] AudioSource _musicAudioSource;
    [SerializeField] PianoManager _pianoManager;

    public bool GameHasStarted;
    public bool gameIsPlaying;

    [SerializeField] GameObject _startCanvasUI;
    [SerializeField] GameObject _playingCanvasUI;
    [SerializeField] List<GameObject> _strikeImages;
    [SerializeField] GameObject _endCanvasUI;
    [SerializeField] GameObject _starfieldVFX;
    [SerializeField] GameObject _neonLines;


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

        // play music only when game has begun
        _musicAudioSource.Play();
        _starfieldVFX.SetActive(true);
        _neonLines.SetActive(true);

        // turning on game play screen
        _playingCanvasUI.SetActive(true);

        _startCanvasUI.SetActive(false);


        gameStarted.Invoke();
        _pianoManager.StartPlayingGame();



    }
    public void ReloadCurrentScene()
    {
        // Get the name of the currently active scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Load the scene by its name
        SceneManager.LoadScene(currentSceneName);
    }
    public void ResetGame()
    {
        gameScore = 0;
        numberOfStrikes = 0;
        scoreText.text = gameScore.ToString();

        gameIsPlaying = false;
        GameHasStarted = false;
        ReloadCurrentScene();

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

            SlowDownGame();

            if (numberOfStrikes == 1) { 
                            _strikeImages[0].SetActive(false);
                _strikeImages[1].SetActive(true);
                _strikeImages[2].SetActive(true);
            }
            
            if (numberOfStrikes == 2) { 
                            _strikeImages[0].SetActive(false);
                _strikeImages[1].SetActive(false);
                _strikeImages[2].SetActive(true);
            }


            if (numberOfStrikes >= 3)
            {
                SlowDownGame();
                _strikeImages[0].SetActive(false);
                _strikeImages[1].SetActive(false);
                _strikeImages[2].SetActive(false);
                EndGame();

            }
        }
    }

    private void EndGame()
    {
        gameIsPlaying = false;

        // play music only when game has begun
        _musicAudioSource.Stop();
        _starfieldVFX.SetActive(false);

        finalScoreText.text = gameScore.ToString();

        _endCanvasUI.SetActive(true);
        _playingCanvasUI.SetActive(false);
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
