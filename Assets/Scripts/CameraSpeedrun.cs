using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpeedrun : MonoBehaviour
{
    private GameObject player;
    private SceneRotation sceneRotation;
    public float speed = 2.0f;
    private bool stopMovement = false;
    public int cameraYaxisOffset = 5;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        sceneRotation = GameObject.Find("GameView").GetComponent<SceneRotation>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!stopMovement) {
            transform.position = new Vector2(player.transform.position.x, player.transform.position.y - cameraYaxisOffset);
        }
    }

    public void StopCamera()
    {
        stopMovement = true;
    }
}
