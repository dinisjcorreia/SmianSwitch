using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scriptNiveis : MonoBehaviour
{

     public class Niveis
    {
    public int nivel;
  
    }

    Niveis niveis;
     string saveFilePath;

     public Button nivel2;
     public Button nivel3;
     public Button boss;

    // Start is called before the first frame update
    void Start()
    {

         saveFilePath = Application.persistentDataPath + "/Niveis.json";
        niveis = new Niveis();
        
        if (File.Exists(saveFilePath))
        {
            string savePlayerData = File.ReadAllText(saveFilePath);
            niveis = JsonUtility.FromJson<Niveis>(savePlayerData);
        } else{
            niveis.nivel = 1;
            string savePlayerData = JsonUtility.ToJson(niveis);
            File.WriteAllText(saveFilePath, savePlayerData);
        }
         if (niveis.nivel == 3){
            boss.interactable = true;
            nivel2.interactable = true;
            nivel2.interactable = true;
         }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Voltar(){
        SceneManager.LoadScene("MainMenu");
    }
}
