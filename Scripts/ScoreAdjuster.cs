using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAdjuster : MonoBehaviour
{
    private GameManager gameManager;

    public void Awake(){
        gameManager = Component.FindObjectOfType<GameManager>();
    }

    public void IncreaseScore(int amountToAdd){
        gameManager.IncreaseScore(amountToAdd);
    }

    public void LoseScore(int amountToLose){
        gameManager.LoseScore(amountToLose);
    }

    public void IncreaseScoreMultiplier(){
        gameManager.IncreaseScoreMultiplier();
    }

    public void ResetScoreMultiplier(){
        gameManager.ResetScoreMultiplier();
    }
}
