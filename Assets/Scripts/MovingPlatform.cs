using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private GameObject go;
    private SceneRotation sceneRotation;
    public float speed = 2f;  
    public float movingDistance = 2;

    public bool moveHorizontally = true;

    void Start()
    {
        go = GameObject.Find("GameView");
        sceneRotation = go.GetComponent<SceneRotation>();
    }

    void Update()
    {
        if(moveHorizontally){
            transform.localPosition = new Vector3(Mathf.PingPong(Time.time * speed, movingDistance), transform.localPosition.y, 0);
        }
        else{
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.PingPong(Time.time * speed, movingDistance), 0);
        }
        
        // if (sceneRotation.isVertical)
        // {
            
        //     // transform.localPosition = new Vector3(Mathf.PingPong(Time.time * speed, movingDistance), transform.localPosition.y, transform.localPosition.z);
        //     transform.localPosition = new Vector3(Mathf.PingPong(Time.time * speed, movingDistance), transform.localPosition.y, 0);
        // }
        // else
        // {
        //     // transform.localPosition = new Vector3(transform.localPosition.x, Mathf.PingPong(Time.time * speed, movingDistance), transform.localPosition.z);
        //     transform.localPosition = new Vector3(transform.localPosition.x, Mathf.PingPong(Time.time * speed, movingDistance), 0);
        // }
    }
}