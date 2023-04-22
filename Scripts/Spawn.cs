using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spawn : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Transform locationToSpawn;
    public spawnMode selectedMode;
    public float spawnRate;
    public int amountToSpawn = 1;
    public bool triggerOnGameplayStart;
    public GameObject lastSpawnedObject {get; private set;}
    private bool isSpawning;
    private GameManager gameManager;

    public void Start(){
        gameManager = GameObject.FindAnyObjectByType<GameManager>().GetComponent<GameManager>();
        if(triggerOnGameplayStart) {
            gameManager.OnMainGamePlayStart.AddListener(SpawnObject);
        }
    }

    public void SpawnObject(){
        switch (selectedMode)
        {
            case spawnMode.spawnContinously:
                SteadySpawn();
                break;
            case spawnMode.spawnSetAmount:
                SpawnSetAmount(amountToSpawn);
                break;
            case spawnMode.spawnOnce:
                SpawnOnce();
                break;
            default:
                break;
        }
    }

    public void SpawnObject(InputAction.CallbackContext context){
        switch (selectedMode)
        {
            case spawnMode.spawnContinously:
                if(!isSpawning && context.phase == InputActionPhase.Started)
                    SteadySpawn();
                else if(isSpawning && context.phase == InputActionPhase.Canceled)
                    SteadySpawn();
                break;
            case spawnMode.spawnSetAmount:
                if(context.phase == InputActionPhase.Started)
                    SpawnSetAmount(amountToSpawn);
                break;
            case spawnMode.spawnOnce:
                if(context.phase == InputActionPhase.Started) SpawnOnce();
                break;
            default:
                break;
        }
    }

    public void SpawnSetAmount(int amount){
        if(!isSpawning){
            StartCoroutine(ISpawnSetAmount(amount));
            isSpawning = true;
        }
        
    }

    private void SteadySpawn(){
        if(isSpawning){
            isSpawning = false;
        }
        else{
            isSpawning = true;
            StartCoroutine(IConstantSpawn());
        }
    }

    private void SpawnOnce(){
        CreateObject();
    }

    private void CreateObject(){
        lastSpawnedObject = Instantiate(objectToSpawn, locationToSpawn.transform.position, locationToSpawn.transform.rotation);
    }

    
    IEnumerator IConstantSpawn(){
        while(isSpawning){
            CreateObject();
            yield return new WaitForSeconds(spawnRate);
        }
    }

    IEnumerator ISpawnSetAmount(int amount){
        int i = 0;
        while(i < amount){
            CreateObject();
            i++;
            yield return new WaitForSeconds(spawnRate);
        }
        isSpawning = false;
    }
}
