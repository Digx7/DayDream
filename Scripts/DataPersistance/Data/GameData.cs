// Stores the relevant game data that will be saved to file

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public SerializableDictionary<string, int> levelHighScores;

    // the values defined in this constructor will be the default vallues
    // the game starts with when there's no data to load
    public GameData(){
        levelHighScores = new SerializableDictionary<string, int>();
    }
}
