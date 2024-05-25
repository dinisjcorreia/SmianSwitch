using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptPausa : MonoBehaviour
{
    public GameObject menuPausa;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            menuPausa.SetActive(true);
        }
        
    }

    public void Retomar(){
        menuPausa.SetActive(false);
    }

    public void Sair(){
        SceneManager.LoadScene("MainMenu");
    }

     public void Definicoes(){
        SceneManager.LoadScene("Definições");
    }
}
