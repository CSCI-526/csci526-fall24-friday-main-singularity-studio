using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ScaleBoxColliderY : MonoBehaviour
{
    // Define the scaling factor for Y-axis
    public float yScaleFactor = 0.45f;

    // Define the Y offset
    public float yOffset = 0.75f;

    void Start()
    {
        // Find all GameObjects in the scene with the name "Spikes"
        GameObject[] spikesObjects = GameObject.FindGameObjectsWithTag("Spike"); // Optionally, you could use FindGameObjectsWithTag("Spike") if all have the tag.

        foreach (GameObject spikesObject in spikesObjects)
        {
            // Find all BoxColliders attached to the child objects of each "Spikes" GameObject
            BoxCollider[] colliders = spikesObject.GetComponentsInChildren<BoxCollider>();

            foreach (BoxCollider collider in colliders)
            {
                // Get the current size of the BoxCollider
                Vector3 size = collider.size;

                // Scale the Y component by the specified factor
                size.y *= yScaleFactor;

                // Apply the new size to the collider
                collider.size = size;

                // Set the Y offset
                Vector3 center = collider.center;
                center.y = yOffset; // Set the Y offset value to 0.75
                collider.center = center;

                // Mark the object as dirty to ensure changes are saved in the scene
                #if UNITY_EDITOR
                EditorUtility.SetDirty(collider);
                #endif
            }
        }
    }
}
