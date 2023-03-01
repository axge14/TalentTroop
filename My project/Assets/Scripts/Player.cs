using System;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField] 
    private float maxHealth = 100f;
    
    [SyncVar]
    private float currentHealth;

    private void Awake()
    {
        SetDefault();
    }

    public void SetDefault()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(transform.name + " a maintenant " + currentHealth + "point de vie");
    }
}
