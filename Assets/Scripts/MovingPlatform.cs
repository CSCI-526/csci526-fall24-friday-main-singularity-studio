using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private GameObject go;
    private bool isVertical = true;
    public float speed = 2f;  
    public float movingDistance = 2;

    void Start()
    {
        go = GameObject.Find("GameView");

        if (go == null)
        {
            Debug.LogError("GameObject 'GameView' not found in the scene!");
            return;
        }

        if (go.TryGetComponent<SceneRotation>(out SceneRotation rotation))
        {
            isVertical = rotation.isVertical;
        }
        else if (go.TryGetComponent<SceneRotationSpeedrun>(out SceneRotationSpeedrun rotationSpeedrun))
        {
            isVertical = rotationSpeedrun.isVertical; 
        }
        else
        {
            Debug.LogError("Neither SceneRotation nor SceneRotationSpeedrun component found on GameView!");
        }
    }

    void Update()
    {
        if (isVertical)
        {
            transform.localPosition = new Vector3(Mathf.PingPong(Time.time * speed, movingDistance), transform.localPosition.y, 0);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.PingPong(Time.time * speed, movingDistance), 0);
        }
    }
}
