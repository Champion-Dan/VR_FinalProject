using UnityEngine;

public class CubeTrigger : MonoBehaviour
{
    public HUDManager hudManager; // Reference to the HUDManager script
    public AudioClip startSound;  // Sound to play when the cube is hit

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow")) // If the arrow hits the cube
        {
            Debug.Log("Cube hit! Starting the game.");

            // Play the start sound if it's assigned
            if (startSound != null)
            {
                AudioSource.PlayClipAtPoint(startSound, transform.position);
            }

            if (hudManager != null)
            {
                hudManager.StartGame(); // Start the game
            }
        }
    }
}
