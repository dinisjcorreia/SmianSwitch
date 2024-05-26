using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
        if (!(SceneManager.GetActiveScene().name == "MainMenu") && !(SceneManager.GetActiveScene().name == "Definições"))
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!(SceneManager.GetActiveScene().name == "MainMenu") && !(SceneManager.GetActiveScene().name == "Definições"))
        {
            Destroy(gameObject);
        }
    }
}
