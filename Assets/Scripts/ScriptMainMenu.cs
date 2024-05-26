using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public class ScriptMainMenu : MonoBehaviour
{
    public Image transitionImage;
    public float rotationSpeed = 180f; // Speed of rotation in degrees per second
    public float maxRotationAngle = 360f; // Maximum rotation angle
    public float scaleSpeed = 0.5f; // Speed of scale increase per second
    public float maxScale = 2f; // Maximum scale of the image

    public float transitionDuration = 1f;

    private bool transitionInProgress = false;

    public class Settings
    {
    public float volume;
    public bool musica;
    public bool efeitossonoros;
    }

    Settings playerData;
    string saveFilePath;
    // Start is called before the first frame update
 public AudioMixer audiolistener;
    

   
private float volume;

    void Start()
    {
        if (transitionImage != null)
        {
            transitionImage.gameObject.SetActive(false); // Initially hide the transition image
        }
        saveFilePath = Application.persistentDataPath + "/Settings.json";
        playerData = new Settings();
        
        if (File.Exists(saveFilePath))
        {
            string savePlayerData = File.ReadAllText(saveFilePath);
            playerData = JsonUtility.FromJson<Settings>(savePlayerData);
        } 
       
         
          if (playerData.volume ==1f){
            volume =0f;
          } else if (playerData.volume ==0f){
            volume=-80f;
          } else if (playerData.volume <1f){
            volume = Mathf.Lerp(-30f, 0f, playerData.volume);
            
          }
          
           audiolistener.SetFloat("Volume", volume);

      

           if (playerData.musica == false){
            audiolistener.SetFloat("volumemusica", -80f);
        } else if (playerData.musica == true){
           audiolistener.SetFloat("volumemusica", 0f);
        }

       

        if (playerData.efeitossonoros == false){
            audiolistener.SetFloat("volumeefeitossonoros", -80f);
        } else if (playerData.efeitossonoros == true){
           audiolistener.SetFloat("volumeefeitossonoros", 0f);
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
