using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class ScriptDefinições : MonoBehaviour
{
    // Start is called before the first frame update
    AudioManager audioManager;
    

    public class Settings
    {
    public float volume;
    public bool musica;
    public bool efeitossonoros;
    }

    Settings playerData;
    
    public GameObject sliderVolume;
    public GameObject checkMusica;
    public GameObject checkEfeitoSonoro;
    public AudioMixer audiolistener;

     string saveFilePath;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

private float volume;
    void Start()
    {
      
       
        saveFilePath = Application.persistentDataPath + "/Settings.json";
        playerData = new Settings();
        
        if (File.Exists(saveFilePath))
        {
            string savePlayerData = File.ReadAllText(saveFilePath);
            playerData = JsonUtility.FromJson<Settings>(savePlayerData);
        } 
        sliderVolume.GetComponent<UnityEngine.UI.Slider>().value = playerData.volume;
         
          if (playerData.volume ==1f){
            volume =0f;
          } else if (playerData.volume ==0f){
            volume=-80f;
          } else if (playerData.volume <1f){
            volume = Mathf.Lerp(-30f, 0f, playerData.volume);
            
          }
          
           audiolistener.SetFloat("Volume", volume);

        checkMusica.GetComponent<UnityEngine.UI.Toggle>().isOn = playerData.musica;

           if (playerData.musica == false){
            audiolistener.SetFloat("volumemusica", -80f);
        } else if (playerData.musica == true){
           audiolistener.SetFloat("volumemusica", 0f);
        }

        checkEfeitoSonoro.GetComponent<UnityEngine.UI.Toggle>().isOn = playerData.efeitossonoros;

        if (playerData.efeitossonoros == false){
            audiolistener.SetFloat("volumeefeitossonoros", -80f);
        } else if (playerData.efeitossonoros == true){
           audiolistener.SetFloat("volumeefeitossonoros", 0f);
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void Sair(){
         Application.Quit();
    }

     public void MenuPrincipal(){
        SceneManager.LoadScene("MainMenu");
    }



    public void SalvarVolume(){
        playerData.volume = (float)sliderVolume.GetComponent<UnityEngine.UI.Slider>().value;
        string savePlayerData = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveFilePath, savePlayerData);
       
        if (playerData.volume ==1f){
            volume =0f;
          } else if (playerData.volume ==0f){
            volume=-80f;
          } else if (playerData.volume <1f){
            volume = Mathf.Lerp(-30f, 0f, playerData.volume);
          }
          
           audiolistener.SetFloat("Volume", volume);
    }

      public void SalvarMusica(){
        playerData.musica = checkMusica.GetComponent<UnityEngine.UI.Toggle>().isOn;
        string savePlayerData = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveFilePath, savePlayerData);

         if (playerData.musica == false){
            audiolistener.SetFloat("volumemusica", -80f);
        } else if (playerData.musica == true){
           audiolistener.SetFloat("volumemusica", 0f);
        }
       
    }

      public void SalvarEfeitosSonoros(){
        playerData.efeitossonoros = checkEfeitoSonoro.GetComponent<UnityEngine.UI.Toggle>().isOn;
        string savePlayerData = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveFilePath, savePlayerData);

        if (playerData.efeitossonoros == false){
            audiolistener.SetFloat("volumeefeitossonoros", -80f);
        } else if (playerData.efeitossonoros == true){
           audiolistener.SetFloat("volumeefeitossonoros", 0f);
        }
       
    }
}
   