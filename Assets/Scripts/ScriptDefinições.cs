using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
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

     string saveFilePath;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

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
        checkMusica.GetComponent<UnityEngine.UI.Toggle>().isOn = playerData.musica;
        checkEfeitoSonoro.GetComponent<UnityEngine.UI.Toggle>().isOn = playerData.efeitossonoros;
      
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
       
        audioManager.PlaySound(audioManager.buttonClickSound);
    }

      public void SalvarMusica(){
        playerData.musica = checkMusica.GetComponent<UnityEngine.UI.Toggle>().isOn;
        string savePlayerData = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveFilePath, savePlayerData);
       
    }

      public void SalvarEfeitosSonoros(){
        playerData.efeitossonoros = checkEfeitoSonoro.GetComponent<UnityEngine.UI.Toggle>().isOn;
        string savePlayerData = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveFilePath, savePlayerData);
       
    }
}
   