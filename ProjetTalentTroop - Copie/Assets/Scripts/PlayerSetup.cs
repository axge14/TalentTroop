using System;
using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] componentsToDisable;

    private Camera sceneCamera;

    [SerializeField] 
    private string remoteLayerName = "RemotePlayer";

    [SerializeField]
    private GameObject playerUIPrefabs;
    private GameObject playerUIInstance;
    
    private void Start()
    {
        if (!isLocalPlayer)
        {
            // désactivation des composants si ce joueur n'est pas le notre
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            // création du UI du joueur local
            playerUIInstance = Instantiate(playerUIPrefabs);
            
        }
        GetComponent<Player>().Setup();
    }

    public override void OnStartClient()
    // méthode qui s'éxecute automatiquement quand un player rejoint la partie
    {
        base.OnStartClient();

        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();
        
        GameManager.RegisterPlayer(netId,player);
    }

    private void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    private void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    private void OnDisable()
    // méthode lu quand un joueur quitte le serveur
    {
        Destroy(playerUIInstance);

        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);
    }
}
