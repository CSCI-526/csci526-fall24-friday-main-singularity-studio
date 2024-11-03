using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStopTrigger : MonoBehaviour
{
    public CameraMovement cameraMovement;

    void OnTriggerEnter2D(Collider2D collision){
        Debug.Log("in side trigger enter from the tutorial stop");
        if (collision.CompareTag("Flag Trigger")){
            Debug.Log("Stop trigger collide into flag");
            cameraMovement.StopCamera();
        }
    }
}
