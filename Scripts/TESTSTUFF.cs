using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TESTSTUFF : MonoBehaviour
{
    public void test(){
        Debug.Log("I Work :)");
    }

    public void testInput(InputAction.CallbackContext context){
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Debug.Log("Started");
                break;
            case InputActionPhase.Performed:
                Debug.Log("Performed");
                break;
            case InputActionPhase.Canceled:
                Debug.Log("Canceled");
                break;
            case InputActionPhase.Disabled:
                Debug.Log("Disabled");
                break;
            case InputActionPhase.Waiting:
                Debug.Log("Waiting");
                break;
            default:
                break;
        }
    }
}
