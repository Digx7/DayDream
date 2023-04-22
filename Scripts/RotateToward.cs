using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateToward : MonoBehaviour
{
    public Transform objectToRotate;
    public bool useLocalPosition;

    private Vector2 directionToRotateToward;

    public void UpdateDirection(Vector2 input){
        directionToRotateToward = input;
    }

    public void UpdateDirection(InputAction.CallbackContext context){
        directionToRotateToward = context.ReadValue<Vector2>();
    }

    public void UpdateCurrentControls(PlayerInput playerInput){
        if(playerInput.currentControlScheme == "Gamepad_Scheme") useLocalPosition = true;
        else useLocalPosition = false;
    }

    private float GetAngle(){
        Vector3 a = Vector3.zero;
        Vector3 b = Vector3.zero;
        
        if(useLocalPosition){
            a = objectToRotate.transform.localPosition;
            b = directionToRotateToward;
        }
        else{
            a = objectToRotate.transform.position;
            b = (Vector2)Camera.main.ScreenToWorldPoint(directionToRotateToward);
        }

        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private bool CheckIfShouldUpdate(){
        if(directionToRotateToward.x < 0.01 && directionToRotateToward.x > -0.01 &&
            directionToRotateToward.y < 0.01 && directionToRotateToward.y > -0.01)
            return false;
        else return true;
    }

    public void FixedUpdate(){
        if(CheckIfShouldUpdate()){
            float angle = GetAngle();
            objectToRotate.rotation =  Quaternion.Euler (new Vector3(0f,0f,angle));
        }
    }
}
