using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        foreach (Transform child in transform)
        {
            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => audioManager.PlaySound(audioManager.buttonClickSound));
            } else
            {
                Toggle toggle = child.GetComponent<Toggle>();
                if (toggle != null)
                {
                    toggle.onValueChanged.AddListener((bool value) => audioManager.PlaySound(audioManager.buttonClickSound));
                }
            }
        }
    }
}
