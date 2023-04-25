using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance {get; private set;}
    public static GameObject player { get; private set;}
    
    [Header("Player Variables")]
    public int lives;
    public Spawn playerSpawnPoint;

    [Header("Events")]
    public UnityEvent OnGameOver; 
    public UnityEvent OnPlayerDeath, OnPlayerRespawn;
    public UnityEvent<int> OnLivesChange, OnCurrentHealthChange, OnMaxHealthChange;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple PlayerManager's found in this scene.  There should only be one.");
        }
        instance = this;
    }

    //Lives methods
    public void GainLife(){
        lives++;
        OnLivesChange.Invoke(1);
    }

    public void LoseLife(){
        lives--;
        OnLivesChange.Invoke(-1);
        if(lives <= 0) OnGameOver.Invoke();
    }
    
    //Other methods
    public void OnGameStart(){
        SpawnPlayer();
    }

    private void SpawnPlayer(){
        playerSpawnPoint.SpawnObject();
        SetUpPlayerObject();
    }

    private void SetUpPlayerObject(){
        player = playerSpawnPoint.lastSpawnedObject;
        Health playerHealth = player.GetComponent<Health>();
        playerHealth.onDeath.AddListener(PlayerDeath);
        playerHealth.onCurrentHealthChange.AddListener(OnCurrentHealthChange.Invoke);
        playerHealth.onMaxHealthChange.AddListener(OnMaxHealthChange.Invoke);
    }

    private void PlayerDeath(){
        LoseLife();
        OnPlayerDeath.Invoke();
        StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer(){
        while(player != null)
            yield return new WaitForSeconds(0.1f);
        if(lives > 0) {
            SpawnPlayer();
            OnPlayerRespawn.Invoke();
        }
    }
}
