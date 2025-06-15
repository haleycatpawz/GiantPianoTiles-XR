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
    private float _normalPitch;

    public bool GameHasStarted;
    public bool gameIsPlaying;

    [SerializeField] GameObject _startCanvasUI;
    [SerializeField] GameObject _playingCanvasUI;
    [SerializeField] List<GameObject> _strikeImages;
    [SerializeField] GameObject _endCanvasUI;

    [SerializeField] GameObject _difficultyScreenUI;

    [SerializeField] GameObject _starfieldVFX;
    [SerializeField] GameObject _neonLines;


    [SerializeField] UnityEvent gameStarted;
    [SerializeField] UnityEvent gameEnded;
    [SerializeField] UnityEvent gameLose;
    [SerializeField] UnityEvent gameWin;

    public enum levelDifficulty
    {
        easy = 0,
        medium = 1,
        hard = 2
    }
    public levelDifficulty gameLevelDifficulty = levelDifficulty.easy;

    private void SetLevelDifficulty(levelDifficulty difficulty)
    {
        gameLevelDifficulty = difficulty;
        if(difficulty == levelDifficulty.easy)
        {
            _pianoManager._moveSpeed = 0.02f;
        }
        
        if(difficulty == levelDifficulty.medium)
        {
            _pianoManager._moveSpeed = 0.04f;
        }
        if(difficulty == levelDifficulty.hard)
        {
            _pianoManager._moveSpeed = 0.06f;
        }

    }

    public void toggleObjectOnOff(GameObject obj)
    {
        if (obj.activeSelf)
        {
            obj.SetActive(false);
        }
        else
        {
            obj.SetActive(true);
        }
    }

   // [SerializeField] 

    public void StartGame()
    {
        gameScore = 0;
        numberOfStrikes = 0;
        scoreText.text = gameScore.ToString();

        gameIsPlaying = true;
        GameHasStarted = true;

        _strikeImages[0].SetActive(true);
        _strikeImages[1].SetActive(true);
        _strikeImages[2].SetActive(true);
        // play music only when game has begun
        _musicAudioSource.Play();


        _starfieldVFX.SetActive(true);
        _neonLines.SetActive(true);

        // turning on game play screen
        _playingCanvasUI.SetActive(true);

        _startCanvasUI.SetActive(false);
        _startCanvasUI.SetActive(false);
        _difficultyScreenUI.SetActive(false);

        gameStarted.Invoke();
        _pianoManager.StartPlayingGame();

        SetLevelDifficulty(gameLevelDifficulty);
    }

    public void ResetGame()
    {
        gameScore = 0;
        numberOfStrikes = 0;
        scoreText.text = gameScore.ToString();
        gameIsPlaying = false;
        GameHasStarted = false;

        _pianoManager.DestroyAllTileSets();

        _starfieldVFX.SetActive(false);
      //  _neonLines.SetActive(false);

        // turning on game play screen
        _startCanvasUI.SetActive(true);

        _endCanvasUI.SetActive(false);
        _playingCanvasUI.SetActive(false);

        _strikeImages[0].SetActive(true);
        _strikeImages[1].SetActive(true);
        _strikeImages[2].SetActive(true);

        //  ReloadCurrentScene();

    }
    public void IncreaseGameScore()
    {
        if (gameIsPlaying)
        {
            gameScore += 100;
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
               // SlowDownGame();
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
        _normalPitch = _musicAudioSource.pitch;
    }

    void AdjustAudioPitch()
    {
            if (Mathf.Approximately(Time.timeScale, 1f))
            {
                _musicAudioSource.pitch = _normalPitch;
            }
            else
            {
                _musicAudioSource.pitch = Time.timeScale;
            }
        }

    void SlowDownMoveSpeed()
    {
        if (Mathf.Approximately(Time.timeScale, 1f))
        {
            _pianoManager._moveSpeed = _pianoManager._originalMoveSpeed;
        }
        else
        {
            _pianoManager._moveSpeed = _pianoManager._moveSpeed * Time.timeScale;
        }
    }


    void Update()
    {
        AdjustAudioPitch();
        SlowDownMoveSpeed();

    }

    private Coroutine timeCoroutine;

    public void SlowDownGame()
    {
        if (gameIsPlaying == true) // toggle slow-mo
        {
            if (timeCoroutine != null)
                StopCoroutine(timeCoroutine);

            if (Mathf.Approximately(Time.timeScale, 1f))
                timeCoroutine = StartCoroutine(ChangeTimeScale(1f, 0.001f, 0.5f)); // Slow time over 1 second
           
            
            // else
             //   timeCoroutine = StartCoroutine(ChangeTimeScale(Time.timeScale, 1f, 0.1f)); // Resume time over 1 second
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
        StartCoroutine(Delay(1));
    }
    
    IEnumerator Delay(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // Use unscaledDeltaTime to remain consistent even when time is slowed
            yield return null;
        }
        timeCoroutine = StartCoroutine(ChangeTimeScale(Time.timeScale, 1f, 0.5f)); // Resume time over 1 second
    }




}
