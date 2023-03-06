using System;
using UnityEngine;
using Mirror;
using System.Collections;

public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;

    [SerializeField] 
    private float maxHealth = 100f;
    
    [SyncVar]
    private float currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;

    private bool[] wasEnabledOnStart;

    public bool isDead{
        get {return _isDead;}
        protected set { _isDead = value;}
    }

    public void Setup()
    {
        wasEnabledOnStart = new bool[disableOnDeath.Length];

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            wasEnabledOnStart[i] = disableOnDeath[i].enabled;
        }

        SetDefault();
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            RpcTakeDamage(50);
        }
    }

    public void SetDefault()
    {
        isDead = false;
        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabledOnStart[i];
        }
    }
    
    [ClientRpc]
    public void RpcTakeDamage(float amount)
    {
        if (isDead)
        {
            return;
        }
        currentHealth -= amount;
        Debug.Log(transform.name + " a maintenant " + currentHealth + "point de vie");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        for (int i = 0; i < disableOnDeath.Length;i++)
        {
            disableOnDeath[i].enabled = false;
        }

        Debug.Log(transform.name + " a été éliminé");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTimer);
        SetDefault();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }
}
