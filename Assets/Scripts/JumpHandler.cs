using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpHandler : MonoBehaviour
{
    [SerializeField] private float jumpForceLandscape = 6.0f;
    [SerializeField] private float jumpForcePortrait = 10.0f;
    [SerializeField] private float normalFallSpeed = 0.05f;
    [SerializeField] private float fallSpeedLandscape = 0.09f;
    private Rigidbody2D rb;
    public SceneRotation sceneRotation;
    private GameObject fireLeft;
    private GameObject fireRight;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fireLeft = GameObject.Find("Fire Left");
        fireRight = GameObject.Find("Fire Right");
        Debug.Log("inside the start jump handler");
        if (fireLeft != null && fireRight != null){
            fireLeft.SetActive(false);
            fireRight.SetActive(false);
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void HandleJump()
    {
        Debug.Log("inside handle jump function");
        
        if (sceneRotation.isVertical && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForcePortrait);
            if (fireLeft != null && fireRight != null && spriteRenderer.sprite.name != "Player_Head"){
                Debug.Log("found fire obj");
                StartCoroutine(DelayStopTime());
            }
            
        }

        else if (!sceneRotation.isVertical && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForceLandscape);
            if (fireLeft != null && fireRight != null && spriteRenderer.sprite.name != "Player_Head"){
                Debug.Log("found fire obj");
                StartCoroutine(DelayStopTime());
            }
            
        }
    }

    private void Update()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += sceneRotation.isVertical 
                ? new Vector2(0, -normalFallSpeed) 
                : new Vector2(0, -fallSpeedLandscape);
        }
    }

    

    IEnumerator DelayStopTime()
    {   
        Debug.Log("inside delay function");
        fireLeft.SetActive(true);
        fireRight.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        fireLeft.SetActive(false);
        fireRight.SetActive(false);
    }
}