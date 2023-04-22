using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public Damage damage;
    public bool destroySelfOnTouch;

    public void OnTriggerEnter2D(Collider2D col){
        if(damage.damageMode == DamageMode.damageOnTouchStart)damageGameObject(col.gameObject);
        else if(damage.damageMode == DamageMode.damageWhileTouching) StartCoroutine(ConstantDamage(col.gameObject));
    }

    public void OnCollisionEnter2D(Collision2D col){
        if(damage.damageMode == DamageMode.damageOnTouchStart)damageGameObject(col.gameObject);
        else if(damage.damageMode == DamageMode.damageWhileTouching) StartCoroutine(ConstantDamage(col.gameObject));
    }

    public void OnTriggerExit2D(Collider2D col){
        if(damage.damageMode == DamageMode.damageOnTouchExit)damageGameObject(col.gameObject);
        else if(damage.damageMode == DamageMode.damageWhileTouching) StopAllCoroutines();
    }

    public void OnCollisionExit2D(Collision2D col){
        if(damage.damageMode == DamageMode.damageOnTouchExit)damageGameObject(col.gameObject);
        else if(damage.damageMode == DamageMode.damageWhileTouching) StopAllCoroutines();
    }

    private void damageGameObject(GameObject _object){
        _object.BroadcastMessage("Damage_Object", damage, SendMessageOptions.DontRequireReceiver);
        if(destroySelfOnTouch) Destroy(this.gameObject);
    }

    IEnumerator ConstantDamage(GameObject _object){
        while(1>0){
            if(_object != null)damageGameObject(_object);
            else yield return null;
            yield return new WaitForSeconds(damage.damageRate);
        }
    }
}
