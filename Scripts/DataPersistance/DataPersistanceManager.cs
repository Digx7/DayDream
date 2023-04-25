// This will manage persistant game data

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;
    private List<IDataPersistance> dataPersistanceObjects;
    private FileDataHandler dataHandler;

    private string selectedProfileId = "";
   
    public static DataPersistanceManager instance {get; private set;}

    private void Awake(){
        if (instance != null){
            Debug.Log("Found more than one Data Persistence Manager in the scene. The newest one is being destroyed");
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        if (disableDataPersistence)
        {
            Debug.LogWarning("Data persistance is being disable!");
        }
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Overrode selected profile id with test id: " + testSelectedProfileId);
        }
    }

    private void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        this.dataPersistanceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene){
        SaveGame();
    }

    public void ChangeSelectedProfileId(string newProfileId){
        // update the profile to use for saving and loading
        this.selectedProfileId = newProfileId;
        // load the game, which will use that profile, updating our game data accordingly
        LoadGame();
    }

    public void NewGame(){
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // return right away if data persistence is disabled
        if (disableDataPersistence)
        {
            return;
        }

        // Load any saved data from a file using the data handler
        this.gameData = dataHandler.Load(selectedProfileId);

        // start a new game if the data is null and we're configured to initialize data for debugging purposes
        if (this.gameData == null && initializeDataIfNull){
            NewGame();
        }

        // if no data can be loaded, don't continue
        if(this.gameData == null){
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded");
            return;
        }

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects){
            dataPersistanceObj.LoadData(gameData);
        }

    }

    public void SaveGame()
    {
        // return right away if data persistence is disabled
        if (disableDataPersistence)
        {
            return;
        }

        // if we don't have any data to save, log a warning here
        if (this.gameData == null){
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved");
            return;
        }

        // pass the data to other scripts so they can update it
        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects){
            dataPersistanceObj.SaveData(ref gameData);
        }

        // timestamp the data so we know when it was last saved
        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        // save that data to a file using the data handler
        dataHandler.Save(gameData, selectedProfileId);
    }

    private void OnApplicationQuit(){
        SaveGame();
    }

    private List<IDataPersistance> FindAllDataPersistenceObjects(){
        IEnumerable<IDataPersistance> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistenceObjects);
    }

    public bool HasGameData(){
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData(){
        return dataHandler.LoadAllProfiles();
    }
}
