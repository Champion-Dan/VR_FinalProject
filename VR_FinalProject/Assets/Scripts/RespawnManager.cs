using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RespawnTarget(GameObject targetPrefab, Vector3 position, Quaternion rotation, float delay)
    {
        StartCoroutine(RespawnCoroutine(targetPrefab, position, rotation, delay));
    }

    private IEnumerator RespawnCoroutine(GameObject targetPrefab, Vector3 position, Quaternion rotation, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Instantiate the target prefab
        if (targetPrefab != null)
        {
            Instantiate(targetPrefab, position, rotation);
            Debug.Log("Target respawned!");
        }
        else
        {
            Debug.LogError("Target prefab is not assigned. Cannot respawn the target.");
        }
    }
}
