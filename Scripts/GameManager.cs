using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour, IDataPersistance
{
    // Variables
    public static GameManager instance {get; private set;}
    [SerializeField] private string levelId;
    [SerializeField] private string nextLevelId = null;

    [ContextMenu("Generate guid for level id")]
    private void GenerateGuid(){
        levelId = System.Guid.NewGuid().ToString();
    }

    [SerializeField] private bool levelHasBeenBeaten = false;
    public int score {get; private set;}
    [SerializeField] private int highScore;
    public int scoreMultiplyer {get; private set;}
    public int currentWave {get; private set;}
    public GamePlayState currentGamePlayState;

    // Events
    public UnityEvent OnIntroStart, OnMainGamePlayStart, OnGameOverScreenStart, OnGameWinScreenStart;
    public UnityEvent<int> OnNextWave, OnScoreUpdate, OnScoreMultiplierUpdate, OnHighScoreUpdate;

    // Start, Awake, and Update Methods
    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple GameManagers found in this scene.  There should only be one.");
        }
        instance = this;


        PlayerManager.instance.OnGameOver.AddListener(StartGameOverScreen);
        ResetScoreMultiplier();
    }
    
    public void Start(){
        _OnUpdateGamePlayState();
        //OnHighScoreUpdate.Invoke(highScore);
    }

    // IDataPersistance Methods
    public void LoadData(GameData data)
    {
        // We check if the key already exits
        // If yes then we just load the value at that key
        if (data.levelHighScores.ContainsKey(levelId))
        {
            highScore = data.levelHighScores[levelId];
        }
        // If no then we load default values
        else
        {
            highScore = 0;
        }

        if (data.levelsBeaten.ContainsKey(levelId))
        {
            levelHasBeenBeaten = data.levelsBeaten[levelId];
        }
        else
        {
            levelHasBeenBeaten = false;
        }
    }

    public void SaveData(ref GameData data)
    {
        // We check if the key already exist
        // If yes then we just update the value at that key
        if (data.levelHighScores.ContainsKey(levelId))
        {
            data.levelHighScores[levelId] = highScore;
        }
        // If no then we add that key value pair
        else
        {
            data.levelHighScores.Add(levelId, highScore);
        }

        // saves weather or not this level has been beaten
        if (data.levelsBeaten.ContainsKey(levelId))
        {
            data.levelsBeaten[levelId] = levelHasBeenBeaten;
        }
        else
        {
            data.levelsBeaten.Add(levelId, levelHasBeenBeaten);
        }

        // saves weather or not the next level has been unlocked
        if (levelHasBeenBeaten && nextLevelId != null)
        {
            if (data.levelsUnlocked.ContainsKey(nextLevelId))
            {
                data.levelsUnlocked[nextLevelId] = true;
            }
            else
            {
                data.levelsUnlocked.Add(nextLevelId, true);
            }
        }
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
                levelHasBeenBeaten = true;
                DataPersistanceManager.instance.SaveGame();
                break;
            default:
                break;
        }
    }

    private void _OnMainGamePlayStart(){
        PlayerManager.instance.OnGameStart();
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
