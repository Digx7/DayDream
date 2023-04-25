using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance {get; private set;}

    [Header("Text objects")]
    public TextMeshProUGUI gamePlayScore, multiplyer, loseScreenCurrentScore, loseScreenHighScore, winScreenCurrentScore, winScreenHighScore, currentWave;
    [Header("Other UI Elements")]
    public Slider healthBar;
    public GameObject livesHolder;
    public GameObject livesUIPrefab;

    private int currentScore, highScore, numberOfLives;
    private int currentHealth = 1;
    private int maxHealth = 1;
    private int scoreMultiplyer = 1;

    private void Awake(){
        if (instance != null)
        {
            Debug.LogWarning("Multiple UIManager's found in this scene.  There should only be one.");
        }
        instance = this;
    }

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

    public void SetMultiplier(int multiplyer){
        scoreMultiplyer = multiplyer;
        DrawScoreMultiplyer();
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

    public void UpdateScoreMultiplyer(int changeToMultiplyer){
        scoreMultiplyer += changeToMultiplyer;
        DrawScoreMultiplyer();
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
        loseScreenCurrentScore.text = "SCORE: " + currentScore;
        winScreenCurrentScore.text = "SCORE: " + currentScore;
    }

    private void DrawHighScore(){
        loseScreenHighScore.text = "HIGH SCORE: " + highScore;
        winScreenHighScore.text = "HIGH SCORE: " + highScore;
    }
    
    private void DrawScoreMultiplyer(){
        multiplyer.text = "x" + scoreMultiplyer;
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
