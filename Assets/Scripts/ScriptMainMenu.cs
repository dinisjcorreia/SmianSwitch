using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScriptMainMenu : MonoBehaviour
{
    public Image transitionImage;
    public float rotationSpeed = 180f; // Speed of rotation in degrees per second
    public float maxRotationAngle = 360f; // Maximum rotation angle
    public float scaleSpeed = 0.5f; // Speed of scale increase per second
    public float maxScale = 2f; // Maximum scale of the image

    public float transitionDuration = 1f;

    private bool transitionInProgress = false;

    // Start is called before the first frame update
    void Start()
    {
        if (transitionImage != null)
        {
            transitionImage.gameObject.SetActive(false); // Initially hide the transition image
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Sair()
    {
        Application.Quit();
    }

    public void Definicoes()
    {
        if (!transitionInProgress)
        {
            StartCoroutine(FadeOutAndLoadScene("Definições"));
        }
    }

    public void Jogar()
    {
        if (!transitionInProgress)
        {
            StartCoroutine(FadeOutAndLoadScene("Primeiro"));
        }
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        transitionInProgress = true;
        transitionImage.gameObject.SetActive(true);

        float angle = 0f;
        while (angle < maxRotationAngle)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            transitionImage.rectTransform.Rotate(Vector3.forward, rotationAmount);
            angle += Mathf.Abs(rotationAmount);

            float scaleAmount = scaleSpeed * Time.deltaTime;
            transitionImage.rectTransform.localScale += new Vector3(scaleAmount, scaleAmount, 0f);
            transitionImage.rectTransform.localScale = Vector3.Min(transitionImage.rectTransform.localScale, new Vector3(maxScale, maxScale, 1f));

            yield return null;
        }

        // Wait for the duration of the transition
        yield return new WaitForSeconds(transitionDuration);

        // Load the next scene
        SceneManager.LoadScene(sceneName);

        // Reset transition variables
        transitionInProgress = false;
    }
}
