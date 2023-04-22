using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement1D : MonoBehaviour
{
    public Transform objectToMove;
    public float speed;
    public Vector3 direction;

    public void FixedUpdate(){
        objectToMove.Translate(direction * speed, objectToMove);
    }
}
