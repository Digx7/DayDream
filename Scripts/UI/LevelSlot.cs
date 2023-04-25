using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSlot : MonoBehaviour, IDataPersistance
{
    [SerializeField] private string levelId = "";
    [SerializeField] private string levelName = "Name";
    [SerializeField] private bool isUnlocked;

    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private GameObject isUnlockedUI; 
    [SerializeField] private GameObject lockedUI;
    [SerializeField] private TextMeshProUGUI nameText, highScoreText;

    private int highScore = 0;

    // In the event no data is loaded we will still update the UI
    public void Start()
    {
        Refresh();
    }

    // refreshes the UI elements
    public void Refresh()
    {
        if(isUnlocked)
        {
            isUnlockedUI.SetActive(true);
            lockedUI.SetActive(false);
            button.interactable = true;

            nameText.text = levelName;
            highScoreText.text = "SCORE: " + highScore;
        }
        else
        {
            isUnlockedUI.SetActive(false);
            lockedUI.SetActive(true);
            button.interactable = false;
        }
    }

    public void LoadData(GameData gameData)
    {
        // if the key doesn't exist, then no data about this level has been saved
        // so don't try to load anything
        if(!gameData.levelsUnlocked.ContainsKey(levelId))
        {
            Refresh();
            return;
        }
        // if the key does exist, then load the data about it
        else
        {
            isUnlocked = gameData.levelsUnlocked[levelId];
            highScore = gameData.levelHighScores[levelId];
        }

        // Will update the UI based on loaded data
        Refresh();
    }

    public void SaveData(ref GameData gameData)
    {
        // if the key doesn't exist, then no data about this level has been saved
        // so add all the data
        if(!gameData.levelsUnlocked.ContainsKey(levelId))
        {
            gameData.levelsUnlocked.Add(levelId, isUnlocked);
            gameData.levelHighScores.Add(levelId, 0);
        }
        // if the key did exist there is nothing else to save
    }
}
