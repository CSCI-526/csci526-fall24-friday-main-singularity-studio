using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialControl : MonoBehaviour
{
    public CameraMovement cameraMovement;
    

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        Debug.Log("start tutorial");
        if(sceneName == "Tutorial"){
            Debug.Log("stop the camera");
            cameraMovement.StopCamera();
        }
    }
    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("Flag Trigger")){
            cameraMovement.MoveCamera();
        }
    }
}
