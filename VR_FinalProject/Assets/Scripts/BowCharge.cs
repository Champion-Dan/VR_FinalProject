using UnityEngine;
using UnityEngine.InputSystem;

public class BowAndArrowMechanic : MonoBehaviour
{
    public InputActionProperty rightTriggerAction;  // Reference to the right trigger input action
    public GameObject arrowPrefab;  // Reference to the arrow prefab
    public Transform bowString;    // Reference to the bowstring object
    public Transform rightHand;    // Reference to the right hand position (to calculate separation distance)
    public Transform leftHand;     // Reference to the left hand position
    public Transform shootPoint;   // Where the arrow is instantiated (near the left hand controller)
    public Transform arrowFront;   // Reference to the front part of the arrow (for proper orientation)
    public float maxPullDistance = 3f;  // Maximum distance to pull the bowstring
    public float maxShootForce = 30f;   // Maximum force of the arrow when shot

    private GameObject currentArrow;  // The arrow that will be shot
    private bool isDrawingBow = false;  // Flag to check if the bow is being drawn
    private float pullDistance = 0f;    // Distance the bowstring is pulled
    private Vector3 initialBowStringPosition;  // Initial position of the bowstring

    private void OnEnable()
    {
        rightTriggerAction.action.Enable();  // Enable the right trigger action
    }

    private void OnDisable()
    {
        rightTriggerAction.action.Disable();  // Disable the action when the object is disabled
    }

    void Start()
    {
        initialBowStringPosition = bowString.localPosition;  // Store the initial position of the bowstring
    }

    void Update()
    {
        float triggerValue = rightTriggerAction.action.ReadValue<float>();

        // Update shootPoint's position and rotation to match the left hand
        if (shootPoint != null && leftHand != null)
        {
            shootPoint.position = leftHand.position;
            shootPoint.rotation = leftHand.rotation; // Set rotation to match the left hand's pointing direction
        }

        if (triggerValue > 0.1f)
        {
            if (!isDrawingBow)
            {
                isDrawingBow = true;

                // Instantiate the arrow at the shoot point, with the correct rotation from the shoot point
                currentArrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);

                // Reset arrow's local position, rotation, and scale
                currentArrow.transform.SetParent(shootPoint); // Attach the arrow to the shoot point
                currentArrow.transform.localPosition = Vector3.zero;  // Make sure it's at the correct position
                currentArrow.transform.localRotation = Quaternion.identity;  // Reset rotation (optional)
                currentArrow.transform.localScale = new Vector3(1f, 1f, 1f);  // Ensure proper scale

                // Ensure the arrow's Rigidbody is initially kinematic while it's being drawn
                Rigidbody arrowRb = currentArrow.GetComponent<Rigidbody>();
                if (arrowRb != null)
                {
                    arrowRb.isKinematic = true;
                }

                // Correct the arrow's rotation to match the shootPoint's forward direction
                currentArrow.transform.rotation = Quaternion.LookRotation(shootPoint.forward, Vector3.up);

                // Adjust the front of the arrow (arrowhead) to match the correct shooting direction
                if (arrowFront != null)
                {
                    // Align the front of the arrow (e.g., the arrowhead) with the shooting direction (Y-axis)
                    arrowFront.rotation = Quaternion.LookRotation(shootPoint.forward, Vector3.up);
                }
            }

            // Calculate the pull distance based on hand separation
            pullDistance = Vector3.Distance(leftHand.position, rightHand.position);
            pullDistance = Mathf.Clamp(pullDistance, 0f, maxPullDistance);

            // Adjust bowstring position based on pull distance
            bowString.localPosition = initialBowStringPosition + new Vector3(0, 0, -pullDistance);
        }
        else if (isDrawingBow)
        {
            // Release the arrow when the trigger is released
            ShootArrow();
            isDrawingBow = false;
        }
    }

    void ShootArrow()
    {
        if (currentArrow != null)
        {
            // Detach the arrow from the shoot point
            currentArrow.transform.SetParent(null);

            // Enable physics for the arrow
            Rigidbody arrowRb = currentArrow.GetComponent<Rigidbody>();
            if (arrowRb != null)
            {
                arrowRb.isKinematic = false;

                // Calculate shooting force based on pull distance
                Vector3 shootDirection = leftHand.forward;  // Use the left hand’s forward direction for shooting
                float appliedForce = Mathf.Lerp(0f, maxShootForce, pullDistance / maxPullDistance);

                // Apply force to the arrow in the correct direction
                arrowRb.velocity = shootDirection * appliedForce; // Similar to your bullet shooting logic

                Debug.Log($"Arrow Shot with Force: {appliedForce}, Direction: {shootDirection}");
            }

            // Optionally destroy the arrow after some time
            Destroy(currentArrow, 5f);
            currentArrow = null;
        }

        // Reset the bowstring position after the shot
        bowString.localPosition = initialBowStringPosition;
    }

    void OnDrawGizmos()
    {
        // Visualize the shoot point in the Scene view
        if (shootPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(shootPoint.position, 0.05f);
            Gizmos.DrawRay(shootPoint.position, shootPoint.forward * 0.5f); // Direction the arrow will shoot
        }

        // Visualize the forward direction of the arrow prefab in the scene view
        if (currentArrow != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(currentArrow.transform.position, currentArrow.transform.forward * 0.5f); // Forward direction of the arrow
        }
    }
}
