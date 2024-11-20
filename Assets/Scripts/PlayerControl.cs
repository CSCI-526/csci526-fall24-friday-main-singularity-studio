using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(JumpHandler))]
[RequireComponent(typeof(SpikeCollision))]
[RequireComponent(typeof(WallCollision))]
[RequireComponent(typeof(LevelCompletion))]
[RequireComponent(typeof(TutorialControl))]
public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D rb;
    private JumpHandler jumpHandler;
    private SpikeCollision spikeCollision;
    private WallCollision wallCollision;
    private LevelCompletion levelCompletion;
    private TutorialControl tutorialControl;
    private bool isGameStarted = false;
    private bool isRolling;
    private bool isTouchingPlatform;
    public Sprite rollingSprite;
    public Sprite notRollingSprite;
    private SpriteRenderer spriteRenderer;
    private float rollingThreshold = 1f;
    // public GameObject Fire;
    private float lastZRotation; 

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        jumpHandler = GetComponent<JumpHandler>();
        spikeCollision = GetComponent<SpikeCollision>();
        wallCollision = GetComponent<WallCollision>();
        levelCompletion = GetComponent<LevelCompletion>();
        tutorialControl = GetComponent<TutorialControl>();
        lastZRotation = transform.rotation.eulerAngles.z;
    }

    private void Update()
    {
        if (isGameStarted)
        {
            // bool isRolling = Mathf.Abs(rb.angularVelocity) > rollingThreshold;
            // print(isRolling);
            float currentZRotation = transform.rotation.eulerAngles.z;
            // if (Mathf.Abs(currentZRotation - lastZRotation) > 10f && isTouchingPlatform) {
            if (isTouchingPlatform) {
                spriteRenderer.sprite = rollingSprite;
                // Fire.SetActive(false);
            }else{
                transform.rotation = Quaternion.Euler(0, 0, 0);
                spriteRenderer.sprite = notRollingSprite;
                // Fire.SetActive(true);
            }
            lastZRotation = currentZRotation;

            // if(isRolling){
            //     spriteRenderer.sprite = rollingSprite;
            //     Fire.SetActive(false);
            // }else{
            //     spriteRenderer.sprite = notRollingSprite;
            //     Fire.SetActive(true);
            // }
            // spriteRenderer.sprite = isRolling ? rollingSprite : notRollingSprite;
            HandleMovement();
            jumpHandler.HandleJump();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Square" || collision.gameObject.name == "Platform Trap Art")
        {
            isTouchingPlatform = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Square" || collision.gameObject.name == "Platform Trap Art")
        {
            isTouchingPlatform = false;
        }
    }

    public void StartGame()
    {
        isGameStarted = true;
        levelCompletion.StartGameTimer();
    }

    private void HandleMovement()
    {
        float moveDirection = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
    }
}
