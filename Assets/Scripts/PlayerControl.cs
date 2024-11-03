using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

[RequireComponent(typeof(JumpHandler))]
[RequireComponent(typeof(SpikeCollision))]
[RequireComponent(typeof(WallCollision))]
[RequireComponent(typeof(LevelCompletion))]
public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D rb;
    private JumpHandler jumpHandler;
    private SpikeCollision spikeCollision;
    private WallCollision wallCollision;
    private LevelCompletion levelCompletion;
    private bool isGameStarted = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpHandler = GetComponent<JumpHandler>();
        spikeCollision = GetComponent<SpikeCollision>();
        wallCollision = GetComponent<WallCollision>();
        levelCompletion = GetComponent<LevelCompletion>();
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
    }

    private void HandleMovement()
    {
        float moveDirection = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
    }
}
