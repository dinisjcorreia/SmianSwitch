using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour
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

    private void OnTriggerEnter(Collider other)
    {
        plataforma.SetActive(true);
       
    }

     void OnTriggerExit(Collider other)
    {
        plataforma.SetActive(false);
    }
}
