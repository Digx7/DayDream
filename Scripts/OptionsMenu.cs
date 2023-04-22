using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public bool shouldSaveAndLoad;
    
    // Public References
    public AudioMixer audioMixer;
    public Slider mainVolumeSlider, musicVolumeSlider, sfxVolumeSlider;
    public TMP_Dropdown qualityDropdown, resolutionDropdown;
    public Toggle fullscreenToggle;

    private Resolution[] resolutions;
 
    

    public void Start(){
        SetUpResolutionDropdown();
        if(shouldSaveAndLoad)LoadOptions();
    }
    
    public void SaveOptions(){
        if(shouldSaveAndLoad)PlayerPrefs.Save();
    }

    public void LoadOptions(){
        if(!shouldSaveAndLoad) return;
        
        if(PlayerPrefs.HasKey("main volume")){
            SetMainVolume(PlayerPrefs.GetFloat("main volume"));
            mainVolumeSlider.value = PlayerPrefs.GetFloat("main volume");
        }
        
        if(PlayerPrefs.HasKey("music volume")){
            SetMusicVolume(PlayerPrefs.GetFloat("music volume"));
            musicVolumeSlider.value = PlayerPrefs.GetFloat("music volume");
        }

        if(PlayerPrefs.HasKey("sfx volume")){
            SetSFXVolume(PlayerPrefs.GetFloat("sfx volume"));
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfx volume");
        }

        if(PlayerPrefs.HasKey("quality index")){
            SetQuality(PlayerPrefs.GetInt("quality index"));
            qualityDropdown.value = PlayerPrefs.GetInt("quality index");
            qualityDropdown.RefreshShownValue();
        }

        if(PlayerPrefs.HasKey("resolution index")){
            SetResolution(PlayerPrefs.GetInt("resolution index"));
            resolutionDropdown.value = PlayerPrefs.GetInt("resolution index");
            resolutionDropdown.RefreshShownValue();
        }

        if(PlayerPrefs.HasKey("fullscreen")){
            if(PlayerPrefs.GetInt("fullscreen") == 1) {
                SetFullScreen(true);
                fullscreenToggle.isOn = true;
            }
            else if(PlayerPrefs.GetInt("fullscreen") == 0){
                SetFullScreen(false);
                fullscreenToggle.isOn = false;
            }
        }
    }

    public void DeleteOptions(){
        PlayerPrefs.DeleteAll();
    }

    public void SetMainVolume(float volume){
        audioMixer.SetFloat("main volume", volume);

        PlayerPrefs.SetFloat("main volume", volume);
    }

    public void SetMusicVolume(float volume){
        audioMixer.SetFloat("music volume", volume);

        PlayerPrefs.SetFloat("music volume", volume);
    }

    public void SetSFXVolume(float volume){
        audioMixer.SetFloat("sfx volume", volume);

        PlayerPrefs.SetFloat("sfx volume", volume);
    }

    public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);

        PlayerPrefs.SetInt("quality index", qualityIndex);
    }

    public void SetResolution(int resolutionIndex){
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        PlayerPrefs.SetInt("resolution index", resolutionIndex);
    }

    public void SetFullScreen(bool isFullScreen){
        Screen.fullScreen = isFullScreen;

        if(isFullScreen)PlayerPrefs.SetInt("fullscreen",1);
        else PlayerPrefs.SetInt("fullscreen",0);
    }

    private void SetUpResolutionDropdown(){
        // Sets up variables
        resolutions = Screen.resolutions;
        List<string> options = new List<string>();
        int currentResolutionsIndex = 0;

        resolutionDropdown.ClearOptions(); // Clears the place holder options

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height){
                currentResolutionsIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionsIndex;
        resolutionDropdown.RefreshShownValue();
    }
}
