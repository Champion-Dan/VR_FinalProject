using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene transitions

public class DoorTransition : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // The scene to load
    [SerializeField] private float triggerDistance = 1.5f; // The distance at which the hand will trigger the door
    public Transform playerHand; // Reference to the player's hand transform (to be assigned in the Inspector)

    private void OnTriggerEnter(Collider other)
    {
        // Ensure that the other object is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            if (playerHand != null) // Ensure the playerHand is assigned
            {
                // Check the distance between the hand and the door
                float distance = Vector3.Distance(playerHand.position, transform.position);
                if (distance <= triggerDistance)
                {
                    Debug.Log("Player hand entered trigger zone! Loading scene: " + sceneToLoad);
                    SceneManager.LoadScene(sceneToLoad);
                }
            }
            else
            {
                Debug.LogError("Player hand is not assigned in the Inspector.");
            }
        }
    }
}


