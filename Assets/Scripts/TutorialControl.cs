using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialControl : MonoBehaviour
{
    public CameraMovement cameraMovement;
    public SceneRotation sceneRotation; 
    public PlayerAppearance playerAppearance;
    public PlayerControl playerControl;

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

        if (collision.gameObject.CompareTag("Flag Trigger")) // Player touch the first flag
        {
            playerAppearance.isSurprise = true;
            StartCoroutine(DelayPlayerMovement(2.5f));
            StartCoroutine(ShowWallCollideInstruction());
            // cameraMovement.MoveCamera();
            Debug.Log("Camera started moving due to Flag Trigger");
        }
        else if (collision.gameObject.CompareTag("Flag Trigger1") && !collidedTriggerList.Contains(collision.gameObject)) // Player touch the flag other than the first one
        {
            StartCoroutine(DelayPlayerMovement(1f));
            StartCoroutine(DelayStopTime());
        }
        else if(collision.gameObject.CompareTag("MoveCameraTrigger")){
            cameraMovement.MoveCamera(); 
            StartCoroutine(DelayMoveTime());
        }
        else if(collision.gameObject.CompareTag("RotateSceneTrigger") && !collidedTriggerList.Contains(collision.gameObject)){
            sceneRotation.RotateScene();
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        collidedTriggerList.Add(collision.gameObject);
    }

    IEnumerator DelayPlayerMovement(float num)
    {
        playerControl.isMoveAble = false;
        yield return new WaitForSeconds(num);
        playerControl.isMoveAble = true;
    }

    IEnumerator DelayMoveTime()
    {
        yield return new WaitForSeconds(1);
        cameraMovement.MoveCamera(); 
    }
    IEnumerator DelayStopTime()
    {
        yield return new WaitForSeconds(1);
        cameraMovement.StopCamera(); 
    }

    IEnumerator ShowWallCollideInstruction()
    {
        cameraMovement.MoveCamera(); 
        yield return new WaitForSeconds(2.5f);
        cameraMovement.StopCamera(); 
        playerAppearance.isSurprise = false;
    }

}