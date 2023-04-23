using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour, IDataPersistance
{
    // Variables
    [SerializeField] private string levelId;

    [ContextMenu("Generate guid for level id")]
    private void GenerateGuid(){
        levelId = System.Guid.NewGuid().ToString();
    }

    public int score {get; private set;}
    private int highScore;
    public int scoreMultiplyer {get; private set;}
    public int currentWave {get; private set;}
    public GamePlayState currentGamePlayState;

    // References
    public PlayerManager playerManager;

    // Events
    public UnityEvent OnIntroStart, OnMainGamePlayStart, OnGameOverScreenStart, OnGameWinScreenStart;
    public UnityEvent<int> OnNextWave, OnScoreUpdate, OnScoreMultiplierUpdate, OnHighScoreUpdate;

    // Start, Awake, and Update Methods
    public void Awake(){
        playerManager.OnGameOver.AddListener(StartGameOverScreen);
        ResetScoreMultiplier();
    }
    
    public void Start(){
        _OnUpdateGamePlayState();
        //OnHighScoreUpdate.Invoke(highScore);
    }

    // IDataPersistance Methods
    public void LoadData(GameData data){
        data.levelHighScores.TryGetValue(levelId, out highScore);
        OnHighScoreUpdate.Invoke(highScore);
    }

    public void SaveData(ref GameData data){
        if (data.levelHighScores.ContainsKey(levelId)){
            data.levelHighScores.Remove(levelId);
        }
        data.levelHighScores.Add(levelId, highScore);
    }

    // Score Methods
    public void IncreaseScore(int amountToAdd){
        score += (amountToAdd * scoreMultiplyer);
        OnScoreUpdate.Invoke(score);
    }

    public void LoseScore(int amountToLose){
        score -= amountToLose;
        if(score < 0) score = 0;
        OnScoreUpdate.Invoke(score);
    }

    public void SetScoreMultiplier(int multiplyer){
        scoreMultiplyer = multiplyer;
        if(scoreMultiplyer <= 0) scoreMultiplyer = 1;
        OnScoreMultiplierUpdate.Invoke(scoreMultiplyer);
    }
    
    public void IncreaseScoreMultiplier(){
        SetScoreMultiplier(scoreMultiplyer + 1);
    }

    public void ResetScoreMultiplier(){
        SetScoreMultiplier(1);
    }

    public void SetHighScore(){
        if(score > highScore) highScore = score;
        OnHighScoreUpdate.Invoke(highScore);
    }

    // Wave Methods
    public void NextWave(){
        currentWave++;
        OnNextWave.Invoke(currentWave);
    }

    public void LastWave(){
        StartCoroutine(ILastWave());
    }
    
    //GamePlayState Methods
    public void UpdateGamePlayState(GamePlayState newState){
        currentGamePlayState = newState;
        _OnUpdateGamePlayState();
    }

    public void StartMainGamePlay(){
        UpdateGamePlayState(GamePlayState.mainGamePlay);
    }

    public void StartGameOverScreen(){
        UpdateGamePlayState(GamePlayState.gameOverScreen);
    }

    private void _OnUpdateGamePlayState(){
        switch (currentGamePlayState)
        {
            case GamePlayState.intro:
                OnIntroStart.Invoke();
                break;
            case GamePlayState.mainGamePlay:
                _OnMainGamePlayStart();
                break;
            case GamePlayState.gameOverScreen:
                OnGameOverScreenStart.Invoke();
                break;
            case GamePlayState.gameWinScreen:
                OnGameWinScreenStart.Invoke();
                SetHighScore();
                break;
            default:
                break;
        }
    }

    private void _OnMainGamePlayStart(){
        playerManager.OnGameStart();
        OnMainGamePlayStart.Invoke();
    }

    private IEnumerator ILastWave(){
        GameObject enemy;
        do
        {
            yield return new WaitForSeconds(1.5f);
            enemy = GameObject.FindWithTag("Enemy");
        } while (enemy != null);
        UpdateGamePlayState(GamePlayState.gameWinScreen);
    }

}
