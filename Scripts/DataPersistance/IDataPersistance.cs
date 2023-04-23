// An interface for something that saves or loads persistant data

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance 
{
    void LoadData(GameData data);

    void SaveData(ref GameData data);
}
