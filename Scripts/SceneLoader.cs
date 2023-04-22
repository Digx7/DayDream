using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByName(string name){
        SceneManager.LoadScene(name);
    }

    public void LoadSceneByIndex(int index){
        SceneManager.LoadScene(index);
    }

    public void ReloadCurrentScene(){
        var currentScene = SceneManager.GetActiveScene();
        LoadSceneByName(currentScene.name);
    }

    public void QuitGame(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        Application.Quit();
    }
}
