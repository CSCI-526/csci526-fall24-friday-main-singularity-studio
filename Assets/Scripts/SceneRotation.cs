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
    
    // Start is called before the first frame update
    void Start()
    {
        nextRotateTime = Time.time + rotationPeriod;
        rotationProgress = 0;

        // UpdateWalls(false);
    }




    // Update is called once per frame
    void Update()
    {
        // check if player reach finish line
        if (!shouldRotate) return;

        // https://stackoverflow.com/questions/42658013/slowly-rotating-towards-angle-in-unity reference rotation

        float currentTime = Time.time;
        //check if we have reach the rotation time
        if(currentTime >= nextRotateTime){
            isRotating = true;

            //check if it currently rotating the scene
            if(rotationProgress < 1 && rotationProgress >= 0){
                rotationProgress += Time.deltaTime;
                // UpdateWalls(true);
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
                }
                else{
                    transform.rotation = Quaternion.Euler(0, 0, 0.0f);
                }

                // set the next rotation time
                nextRotateTime = Time.time + rotationPeriod;
                isVertical = !isVertical;
                rotationProgress = 0;
                isRotating = false;


                // float currentZRotation = transform.eulerAngles.z;

                // float targetZRotation = isVertical ? 90.0f : 0.0f;

                // if (Mathf.Abs(currentZRotation - targetZRotation) > 0.1f)
                // {
                //     float angleDifference = Mathf.LerpAngle(currentZRotation, targetZRotation, Time.deltaTime * 5);
                //     transform.RotateAround(cameraTransform.position, Vector3.forward, angleDifference - currentZRotation);
                // }
                // else
                // {
                //     transform.rotation = Quaternion.Euler(0, 0, targetZRotation);

                //     transform.position = cameraTransform.position + (transform.position - cameraTransform.position).normalized * (transform.position - cameraTransform.position).magnitude;

                //     nextRotateTime = Time.time + rotationPeriod;
                //     isVertical = !isVertical;
                //     rotationProgress = 0;
                //     isRotating = false;
                // }

                // UpdateWalls(false);
            }
        }
    }

    
    public void StopRotation()
    {
        shouldRotate = false;
    }

    public void setCameraOrientation(){
        isVertical = true;
    }

    void UpdateWalls(bool isRotating)
    {
        if (isRotating)
        {
            topWall.SetActive(false);
            bottomWall.SetActive(false);
            leftWall.SetActive(false);
            rightWall.SetActive(false);
        }
        else
        {
            topWall.SetActive(true);
            bottomWall.SetActive(true);
            leftWall.SetActive(true);
            rightWall.SetActive(true);
        }
    }
}