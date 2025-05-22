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
        if (!gameIsPlaying)
        {
            numberOfStrikes += 1;

        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
