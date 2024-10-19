using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureSquare : MonoBehaviour
{
    public Transform leftBorder;
    public Transform rightBorder;
    public Transform topBorder;
    public Transform bottomBorder;

    void Start()
    {
        // Measure horizontal distance
        float width = Vector3.Distance(leftBorder.position, rightBorder.position);
        
        // Measure vertical distance
        float height = Vector3.Distance(topBorder.position, bottomBorder.position);
        
        // Output the results in the console
        Debug.Log("Width: " + width + " | Height: " + height);

        // Check if it is a square
        if (Mathf.Approximately(width, height))
        {
            Debug.Log("The blue region is a square.");
        }
        else
        {
            Debug.Log("The blue region is NOT a square.");
        }
    }
}
