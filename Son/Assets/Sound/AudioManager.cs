using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioClip backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = backgroundMusic;

        audioSource.volume = 0.5f;
        audioSource.loop = true;

        audioSource.Play();

    }

}
