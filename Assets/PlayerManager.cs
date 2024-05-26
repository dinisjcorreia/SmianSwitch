using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] playerPrefabs; // Array of player prefabs
    private GameObject currentPlayer; // The currently active player instance
    private int currentPlayerIndex = 0; // Index to track the current player
    public Transform spawnPoint; // The spawn point for the players

    private void Start()
    {
        // Instantiate the first player at the start
        InstantiatePlayer(currentPlayerIndex);
    }

    // Method to instantiate a player based on the index
    private void InstantiatePlayer(int index)
    {
        if (currentPlayer != null)
        {
            Destroy(currentPlayer); // Destroy the current player instance
        }

        currentPlayer = Instantiate(playerPrefabs[index], spawnPoint.position, spawnPoint.rotation);
    }

    // Method to be called when the boss is hit
    public void OnBossHit()
    {
        // Cycle to the next player
        currentPlayerIndex = (currentPlayerIndex + 1) % playerPrefabs.Length;
        InstantiatePlayer(currentPlayerIndex);
    }
}