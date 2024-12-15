using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandColorChange : MonoBehaviour
{
    public GameObject leftHandSphere;  // Left hand sphere reference
    public GameObject rightHandSphere; // Right hand sphere reference
    private Renderer leftHandRenderer;
    private Renderer rightHandRenderer;

    public Material redMaterial;   // Red material
    public Material greenMaterial; // Green material

    void Start()
    {
        // Ensure the left and right hand spheres are set
        if (leftHandSphere != null)
            leftHandRenderer = leftHandSphere.GetComponent<Renderer>();

        if (rightHandSphere != null)
            rightHandRenderer = rightHandSphere.GetComponent<Renderer>();

        // Set the initial material (red)
        SetMaterial(redMaterial);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered with: {other.gameObject.name}"); // Debug log to track collisions

        // Check if the colliding object is the other hand
        if (other.gameObject == leftHandSphere)
        {
            Debug.Log("Left hand collided with right hand! Changing material to green.");
            SetMaterial(greenMaterial);
        }

        if (other.gameObject == rightHandSphere)
        {
            Debug.Log("Right hand collided with left hand! Changing material to green.");
            SetMaterial(greenMaterial);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log($"Trigger exited with: {other.gameObject.name}"); // Debug log to track exits

        // Reset material when the hands stop colliding
        if (other.gameObject == leftHandSphere || other.gameObject == rightHandSphere)
        {
            Debug.Log("Hands stopped colliding! Changing material to red.");
            SetMaterial(redMaterial);
        }
    }

    private void SetMaterial(Material material)
    {
        // Set the material of both hand spheres
        if (leftHandRenderer != null)
        {
            leftHandRenderer.material = material; // Apply the new material
        }

        if (rightHandRenderer != null)
        {
            rightHandRenderer.material = material; // Apply the new material
        }
    }
}
