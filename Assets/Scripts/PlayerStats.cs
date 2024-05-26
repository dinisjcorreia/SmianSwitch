using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private GameObject
        deathChunkParticle,
        deathBloodParticle;

    private float currentHealth;

    private GameManager GM;

    private void Start()
    {
        currentHealth = maxHealth;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public GameObject vidaminha1;
    public GameObject vidaminha2;
    public GameObject vidaminha3;

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        if(currentHealth <= 0.0f)
        {
            Die();
            vidaminha1.SetActive(false);
            vidaminha2.SetActive(false);
            vidaminha3.SetActive(false);

        } else if (currentHealth <= 10.0f){
            vidaminha1.SetActive(false);
            vidaminha2.SetActive(false);
        } else if (currentHealth <= 20.0f){
             vidaminha1.SetActive(false);
         
        }
    }

    private void Die()
    {
        Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
