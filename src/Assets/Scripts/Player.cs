using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   // config
    [SerializeField] float playerAcceleration = 700f;
    [SerializeField] float jumpSpeed = 300f;
    [SerializeField] float climbSpeed = 700f;
    // state
    bool isAlive = true;
    // cache
    Rigidbody2D playerRigidBody2d;
    Animator playerAnimator;
    LayerMask groundMast;
    LayerMask ladderMask;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerLegsCollider;
    GameSession gameSession;
    void Start()
    {
        playerRigidBody2d = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        groundMast = LayerMask.GetMask("Ground");
        ladderMask = LayerMask.GetMask("Ladder");
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerLegsCollider = GetComponent<BoxCollider2D>();
        gameSession = FindObjectOfType<GameSession>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 12) //enemy layer 
        {
            isAlive = false;
            playerAnimator.SetBool("isRunning", false);
            playerAnimator.SetTrigger("Die");
            gameSession.PlayerDeath();
            PlayerDeathVFX();
        }
    }

    private void PlayerDeathVFX()
    {
        playerRigidBody2d.velocity = 
            new Vector2
            (UnityEngine.Random.Range(-110f,110f),
            UnityEngine.Random.Range(1f, 150f));
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            PlayerRun();
            PlayerJump();
            PlayerClimb();
        }
        else if(playerRigidBody2d!=null)
        {
            if (playerRigidBody2d.velocity.x == 0 && playerRigidBody2d.velocity.y == 0)
            {
                Destroy(playerRigidBody2d);
                Destroy(playerBodyCollider);
                Destroy(playerLegsCollider);
                playerRigidBody2d = null;
            }           
        }
    }

    private void PlayerClimb()
    {
        if (playerLegsCollider.IsTouchingLayers(ladderMask))
        {
            playerRigidBody2d.gravityScale = 0;
            bool isMoving = Math.Abs(Input.GetAxis("Vertical")) > Mathf.Epsilon;
            if (isMoving)
            {
                float direction = Mathf.Sign(Input.GetAxis("Vertical"));
                float climbVal = Time.deltaTime * climbSpeed * direction;
                Vector2 climbVector = new Vector2(playerRigidBody2d.velocity.x, climbVal);
                playerRigidBody2d.velocity = climbVector;
                playerAnimator.SetBool("isLadder", true);
            }
            else
            {
                playerRigidBody2d.velocity = new Vector2(playerRigidBody2d.velocity.x, 0);
                playerAnimator.SetBool("isLadder", false);
            }
        }
        else
        {
            playerRigidBody2d.gravityScale = 1;
            playerAnimator.SetBool("isLadder", false);
        }
    }

    private void PlayerJump()
    {
        if (playerLegsCollider.IsTouchingLayers(groundMast))
        {
            if (Input.GetButtonDown("Jump")) { 
                playerRigidBody2d.velocity = new Vector2(playerRigidBody2d.velocity.x, jumpSpeed);
                playerAnimator.SetTrigger("isJumping");
            }
        }
    }

    private void PlayerRun()
    {
        float horizontaValue = Input.GetAxis("Horizontal");
        bool isMoving = Math.Abs(horizontaValue) > Mathf.Epsilon;
        if (isMoving)
        {
            // left/right sprite flip
            float movingDirection = Mathf.Sign(horizontaValue);
            Vector2 movementVector;
            if (transform.localScale.x != movingDirection)
                transform.localScale = new Vector3(movingDirection, 1, 1);
            // moving
            var horizontallMove = playerAcceleration * horizontaValue * Time.deltaTime;
            movementVector = new Vector2(horizontallMove, playerRigidBody2d.velocity.y);
            playerRigidBody2d.velocity = movementVector;
            playerAnimator.SetBool("isRunning", true);
        } 
        else
        {
            playerRigidBody2d.velocity = new Vector2(0, playerRigidBody2d.velocity.y);;
            playerAnimator.SetBool("isRunning", false);
        }
       
    }
}
