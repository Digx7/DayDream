using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Get2DVectorTowardObject : MonoBehaviour
{
    public Transform _object;
    public string tagToLookFor;
    public Transform reference;
    public UnityEvent<Vector2> directionOfObject;

    private Vector2 direction;
    private bool lookingForObject = false;

    public void Start(){
        if(_object == null)StartCoroutine(FindObject());
    }

    public void Update(){
        if(_object != null){
            lookingForObject = false;
            direction = _object.position - reference.position;
            direction.Normalize();
            directionOfObject.Invoke(direction);
        }
        else StartCoroutine(FindObject());
    }

    IEnumerator FindObject(){
        if(lookingForObject) yield return null;
        while(_object == null){
            lookingForObject = true;
            GameObject g = GameObject.FindWithTag(tagToLookFor);
            if(g != null) _object = g.transform;
            yield return new WaitForSeconds(1.0f);        
        }
    }

}
