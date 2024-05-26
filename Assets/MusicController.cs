using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    void Start()
    {
        audioSource.clip = audioClip;
        audioSource.volume = 0.5f; //MUDAR PARA SETTINGS

        if (audioSource.clip.name.StartsWith("M_")) // M_ for music
        {
            audioSource.loop = true;
        }

        audioSource.Play();
    }
}
