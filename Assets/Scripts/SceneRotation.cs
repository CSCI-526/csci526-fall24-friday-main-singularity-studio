using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRotation : MonoBehaviour
{
    public Transform cameraTransform;
    public bool isVertical = true;
    public bool isRotating = false;
    private bool startRotating = false;
    public float rotationPeriod = 5.0f;
    private float nextRotateTime = 0.0f;
    private float rotationProgress;
    private bool shouldRotate = true;
    private bool isTutoriual = false;

    private float relativePos = 0.0f;
    public GameObject healthDisplay;
    public GameObject levelDisplay;
    public GameObject skipButton;

    
    // Start is called before the first frame update
    void Start()
    {
        nextRotateTime = Time.time + rotationPeriod;
        rotationProgress = 0;
        Scene currentScene = SceneManager.GetActiveScene();

        string sceneName = currentScene.name;
        if (sceneName == "Tutorial"){
            isTutoriual = true;
        }

        if(isVertical)
        {
            relativePos = cameraTransform.position.x - transform.position.x;
        } 
        else
        {
            relativePos = cameraTransform.position.y - transform.position.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // check if player reach finish line
        if (!shouldRotate) return;

        // https://stackoverflow.com/questions/42658013/slowly-rotating-towards-angle-in-unity reference rotation

        float currentTime = Time.time;
        //check if we have reach the rotation time
        if((currentTime >= nextRotateTime && !isTutoriual) || startRotating){
            Debug.Log("start rotating");
            isRotating = true;
            healthDisplay.SetActive(false);
            levelDisplay.SetActive(false);
            if(SceneManager.GetActiveScene().name == "Tutorial"){
                skipButton.SetActive(false);
            }
            //check if it currently rotating the scene
            if(rotationProgress < 1 && rotationProgress >= 0){
                rotationProgress += Time.deltaTime;
                if(isVertical){
                    //rotate the scene from the center of the camera position
                    // https://stackoverflow.com/questions/52737303/in-unity-script-how-to-rotate-and-rotate-to-around-pivot-position reference
                    transform.RotateAround(cameraTransform.position, Vector3.forward, 90.0f * Time.deltaTime);
                }
                else{
                    transform.RotateAround(cameraTransform.position, Vector3.forward, -90.0f * Time.deltaTime);
                }
            }
            else{
                //set the Scene to be 90 degree angle incase of rotation didn't make it perfect 90 degree angle.
                if(isVertical){
                    transform.rotation = Quaternion.Euler(0, 0, 90.0f);
                    Vector3 adjustedPosition = transform.position;
                    adjustedPosition.y = cameraTransform.position.y - relativePos;
                    transform.position = adjustedPosition;
                }
                else{
                    transform.rotation = Quaternion.Euler(0, 0, 0.0f);
                    Vector3 adjustedPosition = transform.position;
                    adjustedPosition.x = cameraTransform.position.x - relativePos;
                    transform.position = adjustedPosition;
                }

                // set the next rotation time
                nextRotateTime = Time.time + rotationPeriod;
                isVertical = !isVertical;
                rotationProgress = 0;
                isRotating = false;
                startRotating = false;
                healthDisplay.SetActive(true);
                levelDisplay.SetActive(true);
                if(SceneManager.GetActiveScene().name == "Tutorial"){
                    skipButton.SetActive(true);
                }
            }
        }
    }

    public void RotateScene(){
        startRotating = true;
        Debug.Log("inside rotate scene function");
    }
    public void StopRotation()
    {
        shouldRotate = false;
    }

    public void setCameraOrientation(){
        isVertical = true;
    }

   
}