using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float angle = 30.0f;
    [SerializeField] private float grabDistance = 1.5f; // Distance within which the character can grab the rope
    [SerializeField] private Transform characterHand; // Reference to the character's hand transform

    private float currentAngle = 0;
    private float timer;
    private bool isCharacterGrabbing = false;

    private void Update()
    {
        if (!isCharacterGrabbing)
        {
            timer += Time.deltaTime * speed;
            float newAngle = Mathf.Sin(timer) * angle;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, newAngle + currentAngle));
        }
        else
        {
            // If the character is grabbing, do nothing to the swing
        }
    }

    private void LateUpdate()
    {
        if (!isCharacterGrabbing && characterHand != null)
        {
            // Check if the character is within range to grab the rope
            float distance = Vector3.Distance(transform.position, characterHand.position);
            if (distance <= grabDistance)
            {
                // Set character's grabbing state to true
                isCharacterGrabbing = true;

                // Parent the character's hand to the rope so it moves with it
                characterHand.parent = transform;
            }
        }
    }
}
