using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shot : MonoBehaviour
{

    [SerializeField]
    private AudioClip sound;

    [SerializeField]
    [Range(0.1f, 1f)] private float volume;

    [SerializeField]
    [Range(0.1f, 2.5f)] private float pitch;

    private AudioSource source;

    [SerializeField]
    private GameObject soundPrefab; // drag and drop the sound prefab from the project folder into this slot

    public Transform bulletSpawnPoint;

    private void Awake()
    {
        gameObject.AddComponent<AudioSource>();
        source = GetComponent<AudioSource>();

        volume = 0.5f;
        pitch = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        source.clip = sound;
        source.volume = volume;
        source.pitch = pitch;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {

                source.PlayOneShot(sound, volume);
            
        }
    }
}
