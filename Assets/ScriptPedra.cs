using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPedra : MonoBehaviour
{
    public GameObject plataforma;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     private void OnCollisionEnter2D(Collision2D collision)
    {
      

         if (collision.gameObject.CompareTag("Botao"))
        {
            plataforma.SetActive(true);
        }
    }

     private void OnCollisionExit2D(Collision2D collision)
    {
       

          if (collision.gameObject.CompareTag("Botao"))
        {
            plataforma.SetActive(false);
        }
    }
}
