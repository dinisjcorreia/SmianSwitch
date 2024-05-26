using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Boss : MonoBehaviour
{
    public PlayerManager playerManager; // Reference to the PlayerManager

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Assuming the players have a tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Notify the PlayerManager that the boss has been hit
            playerManager.OnBossHit();
        }
    }
}
