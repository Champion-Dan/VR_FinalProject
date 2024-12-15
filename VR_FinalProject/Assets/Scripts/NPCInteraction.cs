using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public PromptDialog promptDialog; // Reference to the PromptDialog script

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger zone
        if (other.CompareTag("Player"))
        {
            // Call the ShowPrompt method with the desired message
            promptDialog.ShowPrompt("Shoot the Targets!  Rack up points!  Practice on this one!");
        }
    }
}

