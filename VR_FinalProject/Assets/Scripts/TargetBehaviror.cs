using System.Collections;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    public AudioClip hitSound;       // Sound to play when the target is hit
    public int targetValue = 100;    // Score value for the target
    public Color inactiveColor = Color.red;  // Color to indicate the target is inactive
    private Renderer targetRenderer; // Reference to the target's Renderer component
    private Color originalColor;     // Original color of the target
    private bool isHit = false;      // Flag to track if the target has been hit

    // Movement options
    public enum MovementType
    {
        None,            // No movement
        Oscillation,     // Simple back-and-forth oscillation
        Sinusoidal      // Sinusoidal motion in a circular path
    }

    public MovementType movementType = MovementType.None;  // Type of movement for the target
    public float movementSpeed = 1f;                        // Speed of movement
    public float movementAmplitude = 1f;                    // Amplitude for oscillation or sinusoidal motion
    public Vector3 movementAxis = Vector3.right;           // Axis for oscillation (e.g., X, Y, or Z axis)

    public float inactiveTime = 3f; // How long the target stays inactive (in seconds)

    private Vector3 initialPosition;    // Original position of the target

    // Reference to the HUDManager for updating the score
    public HUDManager hudManager; // Ensure this is linked in the Inspector

    private void Start()
    {
        targetRenderer = GetComponent<Renderer>();
        if (targetRenderer != null)
        {
            originalColor = targetRenderer.material.color;
            Debug.Log("Original color set to: " + originalColor);
        }
        else
        {
            Debug.LogWarning("No Renderer component found on the target object.");
        }

        // Save the initial position for movement
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Handle movement if any movement type is selected
        if (movementType == MovementType.Oscillation)
        {
            Oscillate();
        }
        else if (movementType == MovementType.Sinusoidal)
        {
            SinusoidalMotion();
        }
    }

    private void Oscillate()
    {
        // Oscillation based on sine wave
        float oscillation = Mathf.Sin(Time.time * movementSpeed) * movementAmplitude;
        transform.position = initialPosition + movementAxis * oscillation;
    }

    private void SinusoidalMotion()
    {
        // Sinusoidal motion in a circular path
        float x = Mathf.Sin(Time.time * movementSpeed) * movementAmplitude;
        float y = Mathf.Cos(Time.time * movementSpeed) * movementAmplitude;
        transform.position = initialPosition + new Vector3(x, y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
            Debug.Log("Target hit by arrow!");

            // Only play the hit sound if the target is not in the inactive color
            if (hitSound != null && targetRenderer.material.color != inactiveColor)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }

            HandleHit();
        }
    }

    private void HandleHit()
    {
        // If the target has already been hit, do nothing
        if (isHit)
            return;

        // Change the target's color to the inactive color
        if (targetRenderer != null)
        {
            targetRenderer.material.color = inactiveColor;
            Debug.Log("Target color changed to inactive color.");
        }

        // Mark the target as hit
        isHit = true;

        // Update the score through the HUDManager
        if (hudManager != null)
        {
            Debug.Log("Increasing score by " + targetValue); // Debugging the score increase
            hudManager.IncreaseScore(targetValue);  // Increase score by the target's value
        }
        else
        {
            Debug.LogWarning("HUDManager reference is not assigned.");
        }

        // Start the coroutine to revert the target back to its original color after a delay
        StartCoroutine(RevertTargetColor());
    }

    private IEnumerator RevertTargetColor()
    {
        // Wait for the specified inactive time
        yield return new WaitForSeconds(inactiveTime);

        // Revert the target's color back to the original
        if (targetRenderer != null)
        {
            targetRenderer.material.color = originalColor;
            Debug.Log("Target color reverted to original.");
        }

        // Mark the target as available again for hitting
        isHit = false;
    }
}
