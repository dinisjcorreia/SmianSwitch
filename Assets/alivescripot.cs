using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class alivescripot : MonoBehaviour
{
     public Animator aliveAnim;
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
     public void pararAtaque(){
        aliveAnim.SetBool("Attack", false);
    }
}
