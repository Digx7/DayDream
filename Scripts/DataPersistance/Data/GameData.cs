// Stores the relevant game data that will be saved to file

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public long lastUpdated;
    public SerializableDictionary<string, int> levelHighScores, skinsUnlocked;
    public SerializableDictionary<string, bool>levelsUnlocked, levelsBeaten;

    // the values defined in this constructor will be the default vallues
    // the game starts with when there's no data to load
    public GameData(){
        levelHighScores = new SerializableDictionary<string, int>();
        levelsUnlocked = new SerializableDictionary<string, bool>();
        levelsBeaten = new SerializableDictionary<string, bool>();
        skinsUnlocked = new SerializableDictionary<string, int>();
    }

    public int GetPerentageComplete(){
        // TODO - figure out how many level's we've finished
        // for now we will return a test value
        return 96;
    }

    public int GetNumberOfLevelsBeaten()
    {
        int numberOfLevelsBeaten = 0;
        foreach (KeyValuePair<string, bool> level in levelsBeaten)
        {
            if(level.Value)
            {
                numberOfLevelsBeaten++;
            }
        }
        return numberOfLevelsBeaten;
    }

    public int GetNumberOfTotalLevels()
    {
        int numberOfTotalLevels = 0;
        foreach (KeyValuePair<string, bool> level in levelsBeaten)
        {
            numberOfTotalLevels++;
        }
        return numberOfTotalLevels;
    }
}
