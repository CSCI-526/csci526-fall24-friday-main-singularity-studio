using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public SceneRotation sceneRotation;
    public float speed = 2.0f;
    private bool stopMovement = false;

    public float cameraMovingPeriod = 10.0f;


    // Start is called before the first frame update
    void Start()
    {
        // nextCameraStopTime = Time.time + cameraMovingPeriod;
    }

    // Update is called once per frame
    void Update()
    {
        //stop the camera movement when the scene rotating.
        if (!sceneRotation.isRotating && !stopMovement){
            if(sceneRotation.isVertical){
                transform.position += Vector3.down * speed * Time.deltaTime;
            }
            else{
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
        }
    }

    public void StopCamera()
    {
        stopMovement = true;
    }

    public void MoveCamera(){
        stopMovement = false;
    }
}
