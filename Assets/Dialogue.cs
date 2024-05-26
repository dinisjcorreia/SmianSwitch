using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;
public GameObject vida1;
public GameObject vida2;
public GameObject vida3;
public GameObject vidaminha1;
public GameObject vidaminha2;
public GameObject vidaminha3;
public GameObject vidaminha4;
public GameObject vidaminha5;
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
        vida1.SetActive(false);
        vida2.SetActive(false);
        vida3.SetActive(false);
        vidaminha1.SetActive(false);
        vidaminha2.SetActive(false);
        vidaminha3.SetActive(false);
        vidaminha4.SetActive(false);
        vidaminha5.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }


    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            vida1.SetActive(true);
            vida2.SetActive(true);
            vida3.SetActive(true);
            vidaminha1.SetActive(true);
            vidaminha2.SetActive(true);
            vidaminha3.SetActive(true);
            vidaminha4.SetActive(true);
            vidaminha5.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}