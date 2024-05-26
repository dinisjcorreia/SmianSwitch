using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyProtect : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectsOfType<DestroyProtect>().Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
