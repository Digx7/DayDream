using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public bool shouldSaveAndLoad;
    
    // Public References
    public AudioMixer audioMixer;
    public Slider mainVolumeSlider, musicVolumeSlider, sfxVolumeSlider;
    public TMP_Dropdown qualityDropdown, resolutionDropdown;
    public Toggle fullscreenToggle;
    public GameObject audioFirstSelected, audioCloseSelected, videoFirstSelected, videoCloseSelected, controlsFirstSelected, controlsCloseSelected;

    public GameObject optionsMenuObject, audioObject, videoObject, controlsObject;

    private Resolution[] resolutions;
    private OptionMenu_SubMenus currentSubMenu = OptionMenu_SubMenus.main;
    private bool canTakeInput = false;

    public UnityEvent onOptionsClose;
 
    

    public void Start(){
        SetUpResolutionDropdown();
        if(shouldSaveAndLoad)LoadOptions();
    }
    
    public void GoBack(InputAction.CallbackContext context){
        if(context.phase == InputActionPhase.Performed && canTakeInput){
            switch (currentSubMenu)
            {
                case OptionMenu_SubMenus.main:
                    CloseOptionsMenu();
                    break;
                case OptionMenu_SubMenus.audioMenu:
                    CloseAudioMenu();
                    break;
                case OptionMenu_SubMenus.videoMenu:
                    CloseVideoMenu();
                    break;
                case OptionMenu_SubMenus.controlsMenu:
                    CloseControlsMenu();
                    break;
                default:
                    break;
            }
        }
    }

    public void OpenOptionsMenu(){
        optionsMenuObject.SetActive(true);
        canTakeInput = true;
    }

    public void CloseOptionsMenu(){
        optionsMenuObject.SetActive(false);
        canTakeInput = false;

        SaveOptions();

        onOptionsClose.Invoke();
    }

    public void OpenAudioMenu(){
        optionsMenuObject.SetActive(false);
        audioObject.SetActive(true);

        SetSelectedObject(audioFirstSelected);
    
        currentSubMenu = OptionMenu_SubMenus.audioMenu;
    }

    public void CloseAudioMenu(){
        optionsMenuObject.SetActive(true);
        audioObject.SetActive(false);

        SetSelectedObject(audioCloseSelected);
    
        currentSubMenu = OptionMenu_SubMenus.main;

        SaveOptions();
    }

    public void OpenVideoMenu(){
        optionsMenuObject.SetActive(false);
        videoObject.SetActive(true);

        SetSelectedObject(videoFirstSelected);
    
        currentSubMenu = OptionMenu_SubMenus.videoMenu;
    }

    public void CloseVideoMenu(){
        optionsMenuObject.SetActive(true);
        videoObject.SetActive(false);

        SetSelectedObject(videoCloseSelected);
    
        currentSubMenu = OptionMenu_SubMenus.main;

        SaveOptions();
    }

    public void OpenControlsMenu(){
        optionsMenuObject.SetActive(false);
        controlsObject.SetActive(true);

        SetSelectedObject(controlsFirstSelected);
    
        currentSubMenu = OptionMenu_SubMenus.controlsMenu;
    }

    public void CloseControlsMenu(){
        optionsMenuObject.SetActive(true);
        controlsObject.SetActive(false);

        SetSelectedObject(controlsCloseSelected);
    
        currentSubMenu = OptionMenu_SubMenus.main;

        SaveOptions();
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

    private void SetSelectedObject(GameObject selected){
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selected);

    }

}
