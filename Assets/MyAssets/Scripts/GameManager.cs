using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int gameScore = 0;
    [SerializeField] TMPro.TextMeshPro scoreText;

    [SerializeField] int numberOfStrikes = 0;
   // [SerializeField] 

    private void SetGame()
    {
        gameScore = 0;
        scoreText.text = gameScore.ToString();
        numberOfStrikes = 0;
    }
    
    public void IncreaseGameScore()
    {
        gameScore += 1;
        scoreText.text = gameScore.ToString();
    }

    public void addStrike()
    {
        numberOfStrikes += 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
