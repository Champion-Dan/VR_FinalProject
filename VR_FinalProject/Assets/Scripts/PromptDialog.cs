using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PromptDialog : MonoBehaviour
{
    public TextMeshProUGUI speechText; // Assign in the Inspector
    public GameObject speechBubbleCanvas; // Assign in the Inspector
    public float dialogDuration = 3f; // Duration the dialog will be visible

    private Coroutine dialogCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the speech bubble is hidden initially
        speechBubbleCanvas.SetActive(false);
    }

    // Method to trigger dialog prompt
    public void ShowPrompt(string message)
    {
        speechText.text = message; // Set the speech bubble text
        speechBubbleCanvas.SetActive(true); // Show the speech bubble

        // Start the coroutine to hide the dialog after a certain duration
        if (dialogCoroutine != null)
        {
            StopCoroutine(dialogCoroutine); // Stop any ongoing dialog
        }

        dialogCoroutine = StartCoroutine(HideDialogAfterDelay());
    }

    // Coroutine to hide dialog after a delay
    private IEnumerator HideDialogAfterDelay()
    {
        yield return new WaitForSeconds(dialogDuration);
        speechBubbleCanvas.SetActive(false); // Hide the speech bubble
    }
}
