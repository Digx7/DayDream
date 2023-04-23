using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI percentageCompleteText;

    private Button saveSlotButton;

    private void Awake(){
        saveSlotButton = this.GetComponent<Button>();
    }

    public void SetData(GameData data){
        // there's no data for this profileId
        if (data == null){
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        // there is data for this profileId
        else{
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            percentageCompleteText.text = data.GetPerentageComplete() + "% COMPLETE";
        }
    }

    public string GetProfileId(){
        return this.profileId;
    }

    public void SetInteractable(bool _interactable){
        saveSlotButton.interactable = _interactable;
    }
}
