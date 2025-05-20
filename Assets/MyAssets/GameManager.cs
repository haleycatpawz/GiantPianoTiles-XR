using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int gameScore = 0;
    [SerializeField] TMPro.TextMeshPro scoreText;

    private void SetGameScore()
    {
        gameScore = 0;
        scoreText.text = gameScore.ToString();
    }
    
    private void IncreaseGameScore()
    {
        gameScore += 1;
        scoreText.text = gameScore.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetGameScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
