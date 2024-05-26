using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class ScriptMainMenu : MonoBehaviour
{
    public class Settings
    {
    public float volume;
    public bool musica;
    public bool efeitossonoros;
    }

    Settings playerData;
    string saveFilePath;
    // Start is called before the first frame update
 public AudioMixer audiolistener;
    

   
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
       
         
          if (playerData.volume ==1f){
            volume =0f;
          } else if (playerData.volume ==0f){
            volume=-80f;
          } else if (playerData.volume <1f){
            volume = Mathf.Lerp(-30f, 0f, playerData.volume);
            
          }
          
           audiolistener.SetFloat("Volume", volume);

      

           if (playerData.musica == false){
            audiolistener.SetFloat("volumemusica", -80f);
        } else if (playerData.musica == true){
           audiolistener.SetFloat("volumemusica", 0f);
        }

       

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



     public void Definicoes(){
        SceneManager.LoadScene("Definições");
    }

     public void Jogar(){
        SceneManager.LoadScene("Primeiro");
    }

    

   
}
   