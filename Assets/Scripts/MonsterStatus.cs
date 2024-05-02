using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : MonoBehaviour
{
    public float maxhp;
    public float nowhp;
    public float defense;
    public float speed;
    public float damage;
    public float damageMultiplier;

    public void OnHit(float damage)
    {
        nowhp -= (damage * damageMultiplier - defense);
        Debug.Log(damage * damageMultiplier - defense);
    }
}
