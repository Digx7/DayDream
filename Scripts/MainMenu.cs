using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    
    public static MainMenu instance {get; private set;}
    
    // References
    [Header("Buttons")]
    public Button continueButton;
    public GameObject saveSlotsFirstSelected, saveSlotsCloseSelected, optionsFirstSelected, optionsCloseSelected, creditsFirstSelected, creditsCloseSelected, quitFirstSelected, quitCloseSelected;

    public SaveSlot[] saveSlots;

    [Header("Menus")]
    public OptionsMenu optionsMenu;
    public GameObject mainMenuObject, saveSlotsMenu, creditsObject, quitObject;

    [Header("Texts")]
    public TextMeshProUGUI buildInfoText;

    private MainMenu_SubMenus currentSubMenu = MainMenu_SubMenus.main;

    private bool canTakeInput = true;

    // Awake and Start
    private void Awake(){
        if (instance != null){
            Debug.LogError("Found more than one Main Menu in the scene!");
            return;
        }
        instance = this;
    }

    private void Start(){
        if (!DataPersistanceManager.instance.HasGameData()){
            continueButton.interactable = false;
        }
    }

    // Menu Options
    public void NewGame(){
        // create a new game - which will initialize our game data
        DataPersistanceManager.instance.NewGame();
        // load the gamplay scene  - which will in turn save the game because of
        // OnSceneUnloaded() in the DataPersistanceManager
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void ContinueGame(){
        // Load the next scene - which will in turn load the game because of 
        // OnSceneLoaded() in the DataPersistanceManger
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void LoadGame(){
        OpenSaveSlotsMenu();
    }

    public void OpenSaveSlotsMenu(){
        SetUpSaveSlotsMenu();
        mainMenuObject.SetActive(false);
        saveSlotsMenu.SetActive(true);

        SetSelectedObject(saveSlotsFirstSelected);
        currentSubMenu = MainMenu_SubMenus.saveSlotsMenu;
    }

    public void CloseSaveSlotsMenu(){
        mainMenuObject.SetActive(true);
        saveSlotsMenu.SetActive(false);

        SetSelectedObject(saveSlotsCloseSelected);
        currentSubMenu = MainMenu_SubMenus.main;
    }

    public void OpenOptionsMenu(){
        mainMenuObject.SetActive(false);
        optionsMenu.OpenOptionsMenu();

        canTakeInput = false;

        SetSelectedObject(optionsFirstSelected);
        currentSubMenu = MainMenu_SubMenus.optionsMenu;
    }

    public void CloseOptionsMenu(){
        mainMenuObject.SetActive(true);

        canTakeInput = true;

        SetSelectedObject(optionsCloseSelected);
        currentSubMenu = MainMenu_SubMenus.main;
    }

    public void OpenCreditsMenu(){
        mainMenuObject.SetActive(false);
        creditsObject.SetActive(true);

        SetSelectedObject(creditsFirstSelected);
        currentSubMenu = MainMenu_SubMenus.creditsMenu;
    }

    public void CloseCreditsMenu(){
        mainMenuObject.SetActive(true);
        creditsObject.SetActive(false);

        SetSelectedObject(creditsCloseSelected);
        currentSubMenu = MainMenu_SubMenus.main;
    }

    public void OpenQuitMenu(){
        mainMenuObject.SetActive(false);
        quitObject.SetActive(true);

        SetSelectedObject(quitFirstSelected);
        currentSubMenu = MainMenu_SubMenus.quitMenu;
    }

    public void CloseQuitMenu(){
        mainMenuObject.SetActive(true);
        quitObject.SetActive(false);

        SetSelectedObject(quitCloseSelected);
        currentSubMenu = MainMenu_SubMenus.main;
    }
    
     public void GoBack(InputAction.CallbackContext context){
        if(context.phase == InputActionPhase.Performed && canTakeInput){
            switch (currentSubMenu)
            {
                case MainMenu_SubMenus.main:
                    OpenQuitMenu();
                    break;
                case MainMenu_SubMenus.saveSlotsMenu:
                    CloseSaveSlotsMenu();
                    break;
                case MainMenu_SubMenus.optionsMenu:
                    CloseOptionsMenu();
                    break;
                case MainMenu_SubMenus.creditsMenu:
                    CloseCreditsMenu();
                    break;
                case MainMenu_SubMenus.quitMenu:
                    CloseQuitMenu();
                    break;
                default:
                    break;
            }
        }
    }

    private void SetUpSaveSlotsMenu(){
        // Load all of the profiles that exist
        Dictionary<string, GameData> profilesGameData = DataPersistanceManager.instance.GetAllProfilesGameData();

        // Loop through each save slot in the UI and set the content appropriately
        foreach (SaveSlot saveSlot in saveSlots){
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
        }
    }

    private void SetUpBuildInfoText(){
        buildInfoText.text = Application.version;
    }

    private void SetSelectedObject(GameObject selected){
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selected);

    }

}
