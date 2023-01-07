using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    [SerializeField] AudioClip jumpSFX, dieSFX, bounceSFX, bulletSFX;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;

    public bool isMove = true;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }
    
    void Update()
    {
        if(!isMove) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
        Bounce();
    }
   
    void OnFire(InputValue value)
    {
        if (!isMove) { return; }
        Instantiate(bullet, gun.position, transform.rotation);
        AudioSource.PlayClipAtPoint(bulletSFX, Camera.main.transform.position);
    }
    void OnMove(InputValue value)
    {
        if(!isMove) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if(!isMove) { return; }
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))){ return; }

        if(value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
            AudioSource.PlayClipAtPoint(jumpSFX, Camera.main.transform.position);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);

    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
        
    }

    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return; 
        }

        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void Die()
    {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isMove = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathKick;
            AudioSource.PlayClipAtPoint(dieSFX, Camera.main.transform.position);

            StartCoroutine(FindObjectOfType<GameSession>().ProcessPlayerDeath());
        }
    }

    void Bounce()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Bouncing")) &&
            !myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            AudioSource.PlayClipAtPoint(bounceSFX, Camera.main.transform.position);
        }
    }

}
