using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptMainMenu : MonoBehaviour
{
    // Start is called before the first frame update

    

   


    void Start()
    {
       

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
   