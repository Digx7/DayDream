using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement2D : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;

    private Vector2 movementDirection;

    public void UpdateMovementDirection(Vector2 input){
        movementDirection = input;
    }

    public void UpdateMovementDirection(InputAction.CallbackContext context){
        movementDirection = context.ReadValue<Vector2>();
    }

    public void FixedUpdate(){
        rb.velocity = movementDirection * speed * Time.deltaTime;
    }
}
