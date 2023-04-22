using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public List<DamageTypes> immunities;
    public List<DamageTypes> resistances;
    public List<DamageTypes> weaknesses;
    public bool destroyOnDeath;
    public UnityEvent onDeath;
    public UnityEvent<int> onCurrentHealthChange, onMaxHealthChange;

    public void Damage_Object(Damage damage){
        int trueDamage = calculateTrueAmount(damage);
        currentHealth -= trueDamage;
        onCurrentHealthChange.Invoke(-1 * trueDamage);
        isDead();
    }
    public void Heal_Object(Damage heal){
        int trueHeal = calculateTrueAmount(heal);
        currentHealth += trueHeal;
        onCurrentHealthChange.Invoke(trueHeal);
        isDead();
    }

    public bool isDead(){
        if (currentHealth <= 0) {
            onDeath.Invoke();
            if(destroyOnDeath) {
                onDeath.RemoveAllListeners();
                onCurrentHealthChange.RemoveAllListeners();
                onMaxHealthChange.RemoveAllListeners();
                Destroy(this.gameObject);
            }
            return true;
        }
        else{
           return false; 
        }
    }

    private int calculateTrueAmount(Damage damage){
        int trueAmount = damage.DamageAmount;

        if(doListsHaveAMatchingElement(immunities, damage.damageTypes))
            trueAmount = 0;
        else if(doListsHaveAMatchingElement(resistances, damage.damageTypes))
            trueAmount /= 2;
        else if(doListsHaveAMatchingElement(weaknesses, damage.damageTypes))
            trueAmount *= 2;

        return trueAmount;
    }

    private bool doListsHaveAMatchingElement(List<DamageTypes> list1, List<DamageTypes> list2){
        foreach (DamageTypes type in list1){
            if(list2.Contains(type)) return true;
        }
        return false;
    }

}
