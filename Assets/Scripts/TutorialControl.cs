using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialControl : MonoBehaviour
{
    public CameraMovement cameraMovement;
    public SceneRotation sceneRotation; 

    private HashSet<GameObject> collidedTriggerList = new HashSet<GameObject>();
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        string sceneName = currentScene.name;
        if (sceneName == "Tutorial"){
            cameraMovement.StopCamera();
            Debug.Log("Camera stopped at the start of the tutorial scene");
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Flag Trigger"))
        {
            cameraMovement.MoveCamera();
            Debug.Log("Camera started moving due to Flag Trigger");
        }
        else if (collision.gameObject.CompareTag("Flag Trigger1") && !collidedTriggerList.Contains(collision.gameObject))
        {
            StartCoroutine(DelayStopTime());
        }
        else if(collision.gameObject.CompareTag("MoveCameraTrigger")){
            cameraMovement.MoveCamera(); 
        }
        else if(collision.gameObject.CompareTag("RotateSceneTrigger") && !collidedTriggerList.Contains(collision.gameObject)){
            sceneRotation.RotateScene();
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        collidedTriggerList.Add(collision.gameObject);
    }

    IEnumerator DelayStopTime()
    {
        yield return new WaitForSeconds(1);
        cameraMovement.StopCamera(); 
    }
}