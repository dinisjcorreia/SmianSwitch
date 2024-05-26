using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    //Audio
    [SerializeField] private AudioSource audioSourceOnce;
    [SerializeField] private AudioSource audioSourceLoop;

    [SerializeField] public AudioClip[] musicClips;
    [SerializeField] public AudioClip jumpSound;
    [SerializeField] public AudioClip hitSound;
    [SerializeField] public AudioClip swordSound;
    [SerializeField] public AudioClip dashSound;
    [SerializeField] public AudioClip buttonClickSound;

    private Dictionary<string, AudioClip> musicScenes = new Dictionary<string, AudioClip>();
    private void Awake()
    {
        musicScenes.Add("MainMenu", musicClips[0]);
        musicScenes.Add("Definições", musicClips[0]);
        musicScenes.Add("Primeiro", musicClips[1]);
        musicScenes.Add("Main", musicClips[2]);
        musicScenes.Add("Terceiro", musicClips[3]);
        musicScenes.Add("BOSS", musicClips[4]);
        musicScenes.Add("PVP", musicClips[4]);
    }
    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    // Start is called before the first frame update
    void Start()
    {
        audioSourceLoop.loop = true;
        audioSourceLoop.clip = musicScenes[SceneManager.GetActiveScene().name];
        audioSourceLoop.Play();
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSourceOnce.loop = false;
        audioSourceOnce.PlayOneShot(audioClip);
    }

    public bool CheckPlaying()
    {
        if (audioSourceOnce.isPlaying)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (audioSourceLoop.clip == musicScenes[scene.name])
        {
            return;
        }
        audioSourceLoop.Stop();
        audioSourceLoop.clip = musicScenes[scene.name];
        audioSourceLoop.Play();
    }
}
