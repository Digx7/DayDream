using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Damage
{
    public int DamageAmount;
    public List<DamageTypes> damageTypes;
    public DamageMode damageMode;
    public float damageRate;
}
