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
    // References
    public Button continueButton;
    public TextMeshProUGUI buildInfoText;
    public GameObject optionsFirstSelected, optionsCloseSelected, creditsFirstSelected, creditsCloseSelected, quitFirstSelected, quitCloseSelected;

    public GameObject mainMenuObject, creditsObject, quitObject;
    public OptionsMenu optionsMenu;

    private MainMenu_SubMenus currentSubMenu = MainMenu_SubMenus.main;

    private bool canTakeInput = true;

    private void Start(){
        if (!DataPersistanceManager.instance.HasGameData()){
            continueButton.interactable = false;
        }
    }

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

    public void GoBack(InputAction.CallbackContext context){
        if(context.phase == InputActionPhase.Performed && canTakeInput){
            switch (currentSubMenu)
            {
                case MainMenu_SubMenus.main:
                    OpenQuitMenu();
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
    

    private void SetUpBuildInfoText(){
        buildInfoText.text = Application.version;
    }

    private void SetSelectedObject(GameObject selected){
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selected);

    }

}
