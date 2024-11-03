using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialControl : MonoBehaviour
{
    private CameraMovement cameraMovement;

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if(sceneName == "Tutorial"){
            cameraMovement.StopCamera();
        }
    }
    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("FlagTrigger")){
            cameraMovement.MoveCamera();
        }
    }
}
