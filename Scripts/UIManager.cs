using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI gamePlayScore, endScreenCurrentScore, endScreenHighScore, currentWave;
    public Slider healthBar;
    public GameObject livesHolder;
    public GameObject livesUIPrefab;

    private int currentScore, highScore, numberOfLives;
    private int currentHealth = 1;
    private int maxHealth = 1;

    public void Start(){
        ResetHealthValues();
        ResetLivesValues();
    }

    // Macro Functions ============

    public void ResetCurrentScore(){
        currentScore = 0;
        DrawCurrentScore();
    }
    
    public void ResetHealthValues(){
        currentHealth = 10;
        maxHealth = 10;
        DrawHealth();
    }

    public void ResetLivesValues(){
        numberOfLives = 3;
        DrawLives();
    }


    // Set =====================
    public void SetCurrentScore(int score){
        currentScore = score;
        DrawCurrentScore();
    }

    public void SetHighScore(int score){
        highScore = score;
        DrawHighScore();
    }

    public void SetLives(int lives){
        numberOfLives = lives;
        DrawLives();
    }
    
    public void SetCurrentHealth(int health){
        currentHealth = health;
        DrawHealth();
    }

    public void SetMaxHealth(int health){
        maxHealth = health;
        if(maxHealth <= 0) maxHealth = 1;
        DrawHealth();
    }

    public void SetCurrentWave(int wave){
        currentWave.text = "Wave " + wave;
    }

    // Update ======================
    public void UpdateCurrentScore(int changeToScore){
        currentScore += changeToScore;
        DrawCurrentScore();
    }

    public void UpdateHighScore(int changeToScore){
        highScore += changeToScore;
        DrawHighScore();
    }

    public void UpdateLives(int changeToLives){
        numberOfLives += changeToLives;
        DrawLives();
    }

    public void UpdateCurrentHealth(int changeToHealth){
        currentHealth += changeToHealth;
        DrawHealth();
    }

    public void UpdateToMaxHealth(int changeToHealth){
        maxHealth += changeToHealth;
        DrawHealth();
    }

    // Draw ==========================
    private void DrawCurrentScore(){
        gamePlayScore.text = "" + currentScore;
        endScreenCurrentScore.text = "" + currentScore;
    }

    private void DrawHighScore(){
        endScreenHighScore.text = "" + highScore;
    }
    
    private void DrawLives(){
        foreach (Transform child in livesHolder.transform)
        {
            Destroy(child.gameObject);
        }
        for(int i = 0; i < numberOfLives; ++i){
            Instantiate(livesUIPrefab, livesHolder.transform);
        }
    }

    private void DrawHealth(){
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

}
