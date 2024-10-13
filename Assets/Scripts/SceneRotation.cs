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
    
    // Start is called before the first frame update
    void Start()
    {
        nextRotateTime = Time.time + rotationPeriod;
        rotationProgress = 0;
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

            //check if it currently rotation the scene
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
                }
                else{
                    transform.rotation = Quaternion.Euler(0, 0, 0.0f);
                }

                //set the next rotation time
                nextRotateTime = Time.time + rotationPeriod;
                isVertical = !isVertical;
                rotationProgress = 0;
                isRotating = false;
            }
        }
    }

    
    public void StopRotation()
    {
        shouldRotate = false;
    }
}