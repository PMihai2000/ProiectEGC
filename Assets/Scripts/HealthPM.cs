using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPM : MonoBehaviour
{
 
    public int maxHP = 100;
    public int currentHP;
    public bool IsAlive => currentHP > 0;

   

    public void Increment(int value = 1)
    {
        currentHP = Mathf.Clamp(currentHP + value, 0, maxHP);
    }

    /// <summary>
    /// Decrement the HP of the entity. Will trigger a HealthIsZero event when
    /// current HP reaches 0.
    /// </summary>
    public void Decrement(int value = 1)
    {
        currentHP = Mathf.Clamp(currentHP-value,0,maxHP);
        if (currentHP == 0)
        {
            //Vedem percurs :))
        }
    }

    /// <summary>
    /// Decrement the HP of the entitiy until HP reaches 0.
    /// </summary>
    public void Die()
    {
        while (currentHP > 0) Decrement();
    }

    void Awake()
    {
        currentHP = maxHP;
    }
}