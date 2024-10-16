using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;  
    public float distance = 3f;  
    public bool Vertical = false;  

    private float min;  
    private float max;  

    void Start()
    {
        if (Vertical)
        {
            min = transform.position.y;
            max = transform.position.y + distance;
        }
        else
        {
            min = transform.position.x;
            max = transform.position.x + distance;
        }
    }

    void Update()
    {
        if (Vertical)
        {
            transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time * speed, max - min) + min, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(Mathf.PingPong(Time.time * speed, max - min) + min, transform.position.y, transform.position.z);
        }
    }
}
