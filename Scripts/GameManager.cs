using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // Variables
    public int score {get; private set;}
    public int currentWave {get; private set;}
    public GamePlayState currentGamePlayState;

    // References
    public PlayerManager playerManager;

    // Events
    public UnityEvent OnIntroStart, OnMainGamePlayStart, OnGameOverScreenStart, OnGameWinScreenStart;
    public UnityEvent<int> OnNextWave;

    // Start, Awake, and Update Methods
    public void Awake(){
        playerManager.OnGameOver.AddListener(StartGameOverScreen);
    }
    
    public void Start(){
        _OnUpdateGamePlayState();
    }

    // Score Methods
    public void IncreaseScore(int amountToAdd){
        score += amountToAdd;
    }

    public void LoseScore(int amountToLose){
        score -= amountToLose;
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
