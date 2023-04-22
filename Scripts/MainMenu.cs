using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    // References
    public TextMeshProUGUI buildInfoText;

    private void SetUpBuildInfoText(){
        buildInfoText.text = Application.version;
    }
}
