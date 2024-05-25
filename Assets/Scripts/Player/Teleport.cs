using UnityEngine;

public class Teleport : MonoBehaviour
{
    // The destination where the player will be teleported
    [SerializeField]
    private Transform destination;

    // Public method to get the destination
    public Transform GetDestination()
    {
        return destination;
    }
}
