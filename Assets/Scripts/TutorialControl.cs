using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialControl : MonoBehaviour
{
    private bool isPaused = false; 
    public CameraMovement cameraMovement; 
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        string sceneName = currentScene.name;
        if (sceneName == "Tutorial")
            cameraMovement.StopCamera();
            Debug.Log("Camera stopped at the start of the tutorial scene");
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Flag Trigger"))
        {
            cameraMovement.MoveCamera();
            Debug.Log("Camera started moving due to Flag Trigger");
        }

        
        else if (collision.gameObject.CompareTag("Flag Trigger1") && !isPaused)
        {
            StartCoroutine(PauseAndContinueCamera());
        }
    }

    private IEnumerator PauseAndContinueCamera()
    {
        isPaused = true; 

        cameraMovement.StopCamera(); 
        Debug.Log("Camera paused due to Flag Trigger1");

        yield return new WaitForSeconds(1); 

        cameraMovement.MoveCamera();
        Debug.Log("Camera resumed moving after pause");

        isPaused = false; 
    }
}