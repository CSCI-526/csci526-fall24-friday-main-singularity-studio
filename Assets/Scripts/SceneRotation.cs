using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRotation : MonoBehaviour
{
    public Transform cameraTransform;
    public bool isVertical = true;
    public bool isRotating = false;
    public float rotationPeriod = 5.0f;
    private float nextRotateTime = 0.0f;
    private float rotationProgress;
    private bool shouldRotate = true;

    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject topWall;
    public GameObject bottomWall;

    public Transform LeftBorder;
    public Transform RightBorder;
    public Transform TopBorder;
    public Transform BottomBorder;

  
    public float wallThickness = 0.0f;
    public float borderThickness = 0.0f;

    void Start()
    {
        nextRotateTime = Time.time + rotationPeriod;
        rotationProgress = 0;

        // Adjust walls based on border objects
        AdjustCamera();

        UpdateWalls();
    }

    void Update()
    {
        if (!shouldRotate) return;

        float currentTime = Time.time;

        // Check if it's time to rotate
        if (currentTime >= nextRotateTime)
        {
            isRotating = true;

            if (rotationProgress < 1 && rotationProgress >= 0)
            {
                rotationProgress += Time.deltaTime;
                
                // Rotate
                if (isVertical)
                {
                    transform.RotateAround(cameraTransform.position, Vector3.forward, 90.0f * Time.deltaTime);
                }
                else
                {
                    transform.RotateAround(cameraTransform.position, Vector3.forward, -90.0f * Time.deltaTime);
                }
            }
            else
            {
                if (isVertical)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 90.0f);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0.0f);
                }

                nextRotateTime = Time.time + rotationPeriod;
                isVertical = !isVertical;
                rotationProgress = 0;
                isRotating = false;

                AdjustCamera();

                
                UpdateWalls();
            }
        }
    }

    void AdjustCamera()
    {
        Camera mainCamera = Camera.main;

        // Adjust camera based on walls
        if (mainCamera.orthographic)
        {
            float left = leftWall.transform.position.x;
            float right = rightWall.transform.position.x;
            float top = topWall.transform.position.y;
            float bottom = bottomWall.transform.position.y;

            left -= wallThickness;
            right += wallThickness;
            top += wallThickness;
            bottom -= wallThickness;

            float cameraBorderWidth = 0f;
            float cameraBorderHeight = 0f;

            // Adjust camera border based on border objects
            if (LeftBorder != null && RightBorder != null)
            {
                SpriteRenderer leftRenderer = LeftBorder.GetComponent<SpriteRenderer>();
                SpriteRenderer rightRenderer = RightBorder.GetComponent<SpriteRenderer>();
                if (leftRenderer != null && rightRenderer != null)
                {
                    cameraBorderWidth = leftRenderer.bounds.size.x + rightRenderer.bounds.size.x + 2 * borderThickness;
                }
            }

            if (TopBorder != null && BottomBorder != null)
            {
                SpriteRenderer topRenderer = TopBorder.GetComponent<SpriteRenderer>();
                SpriteRenderer bottomRenderer = BottomBorder.GetComponent<SpriteRenderer>();
                if (topRenderer != null && bottomRenderer != null)
                {
                    cameraBorderHeight = topRenderer.bounds.size.y + bottomRenderer.bounds.size.y + 2 * borderThickness;
                }
            }

            float totalWidth = right - left + cameraBorderWidth;
            float totalHeight = top - bottom + cameraBorderHeight;

            float centerX = (left + right) / 2;
            float centerY = (top + bottom) / 2;
            mainCamera.transform.position = new Vector3(centerX, centerY, mainCamera.transform.position.z);

          
            if (isVertical)
            {
               
                float aspectRatio = mainCamera.aspect;
                mainCamera.orthographicSize = totalWidth / (2 * aspectRatio);
            }
            else
            {
            
                mainCamera.orthographicSize = totalHeight / 2;
            }
        }
        
    }

    void UpdateWalls()
    {
        if (isVertical)
        {
            
            topWall.SetActive(true);
            bottomWall.SetActive(true);
            leftWall.SetActive(false);
            rightWall.SetActive(false);
        }
        else
        {
            
            topWall.SetActive(false);
            bottomWall.SetActive(false);
            leftWall.SetActive(true);
            rightWall.SetActive(true);
        }
    }

    public void StopRotation()
    {
        shouldRotate = false;
    }
}
