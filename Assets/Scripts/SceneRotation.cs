using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneRotation : MonoBehaviour
{
    public event System.Action OnFlashBeforeRotation;

    public TextMeshProUGUI countdownText;
    public Transform cameraTransform;
    public bool isVertical = true;
    public bool isRotating = false;
    private bool startRotating = false;
    public float rotationPeriod = 5.0f;
    private float nextRotateTime = 0.0f;
    private float nextFlashTime = 0.0f;
    public float timeFlashBeforeRotation = 1.0f;
    private float rotationProgress;
    private bool shouldRotate = true;
    private bool isTutoriual = false;
    public CameraMovement cameraMovement;

    private float relativePos = 0.0f;
    public GameObject healthDisplay;
    public GameObject levelDisplay;
    public GameObject skipButton;

    void Start()
    {
        if (isVertical)
        {
            relativePos = cameraTransform.position.x - transform.position.x;
        }
        else
        {
            relativePos = cameraTransform.position.y - transform.position.y;
        }

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            isTutoriual = true;
            countdownText.gameObject.SetActive(false);
        }
        else
        {
            cameraMovement.StopCamera();
            StartCoroutine(CountdownCoroutine());
        }
    }

    private IEnumerator CountdownCoroutine()
    {
        nextRotateTime = Time.time + rotationPeriod;
        nextFlashTime = nextRotateTime - timeFlashBeforeRotation;
        rotationProgress = 0;
        countdownText.gameObject.SetActive(true);

        // Wait for 3 seconds before flashing
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(.5f); // Wait for 0.5 seconds
        }

        countdownText.gameObject.SetActive(false);
        cameraMovement.MoveCamera();
    }

    void Update()
    {
        if (!shouldRotate) return;

        float currentTime = Time.time;

        // emit flash event
        if (currentTime >= nextFlashTime && !isTutoriual) 
        {
            OnFlashBeforeRotation?.Invoke();

            // dirty
            nextFlashTime = nextRotateTime + rotationPeriod;  // not actual time, will be updated after rotation for the actual time
        }

        if ((currentTime >= nextRotateTime && !isTutoriual) || startRotating)
        {
            isRotating = true;
            healthDisplay.SetActive(false);
            levelDisplay.SetActive(false);
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                skipButton.SetActive(false);
            }

            if (rotationProgress < 1 && rotationProgress >= 0)
            {
                rotationProgress += Time.deltaTime;
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
                    Vector3 adjustedPosition = transform.position;
                    adjustedPosition.y = cameraTransform.position.y - relativePos;
                    transform.position = adjustedPosition;
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0.0f);
                    Vector3 adjustedPosition = transform.position;
                    adjustedPosition.x = cameraTransform.position.x - relativePos;
                    transform.position = adjustedPosition;
                }

                nextRotateTime = Time.time + rotationPeriod;
                nextFlashTime = nextRotateTime - timeFlashBeforeRotation;
                isVertical = !isVertical;
                rotationProgress = 0;
                isRotating = false;
                startRotating = false;
                healthDisplay.SetActive(true);
                levelDisplay.SetActive(true);
                if (SceneManager.GetActiveScene().name == "Tutorial")
                {
                    skipButton.SetActive(true);
                }
            }
        }
    }

    public void RotateScene()
    {
        startRotating = true;
        Debug.Log("inside rotate scene function");
    }

    public void StopRotation()
    {
        shouldRotate = false;
    }

    public void setCameraOrientation()
    {
        isVertical = true;
    }
}
