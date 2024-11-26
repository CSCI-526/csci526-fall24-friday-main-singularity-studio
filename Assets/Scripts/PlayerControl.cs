using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(JumpHandler))]
[RequireComponent(typeof(SpikeCollision))]
[RequireComponent(typeof(WallCollision))]
[RequireComponent(typeof(LevelCompletion))]
[RequireComponent(typeof(TutorialControl))]
[RequireComponent(typeof(PlayerAppearance))]
public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D rb;
    private JumpHandler jumpHandler;
    private SpikeCollision spikeCollision;
    private WallCollision wallCollision;
    private LevelCompletion levelCompletion;
    private TutorialControl tutorialControl;
    private PlayerAppearance playerAppearance;
    private bool isGameStarted = false;
    public bool isMoveAble = true;
    private float rollingThreshold = 1f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpHandler = GetComponent<JumpHandler>();
        spikeCollision = GetComponent<SpikeCollision>();
        wallCollision = GetComponent<WallCollision>();
        levelCompletion = GetComponent<LevelCompletion>();
        tutorialControl = GetComponent<TutorialControl>();
        playerAppearance = GetComponent<PlayerAppearance>();
    }

    private void Update()
    {
        if (isGameStarted)
        {
            HandleMovement();
            jumpHandler.HandleJump();
        }
    }

    public void StartGame()
    {
        isGameStarted = true;
        levelCompletion.StartGameTimer();
    }

    private void HandleMovement()
    {
        if(isMoveAble){
            float moveDirection = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
        }
        else{
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
}
