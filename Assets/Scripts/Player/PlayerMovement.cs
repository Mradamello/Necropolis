using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float crouchSpeed;
    private float standingCollSizeY;
    private float standingCollOffsetY;
    private float crouchingCollSizeY;
    private float crouchingCollOffsetY;
    private bool facingRight = true;
    public bool isCrouched = false;
    private float dirX;
    private StaminaSystem staminaSystem;
    [SerializeField] private float jumpStaminaRequirement;
    private TrailRenderer trailRenderer;

    [Header("Wallslide")]
    [SerializeField] private float timeBeforeSlide;
    [Range(0.1f, 0.9f)]
    [SerializeField] private float wallSlideSpeedModifier;
    [SerializeField] private float wallJumpVelocity;
    [SerializeField] private float wallJumpTime;
    [SerializeField] private Transform wallCheckPos;
    [SerializeField] private Vector2 wallCheckSize = new Vector2(.49f, .03f);
    private float wallStickTime;
    private bool wallJumping = false;
    private bool canDoWallJump = false;
    private bool isStickingToWall = false;
    private bool canStickToWall = false;

    [Header("Dashing")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] public float dashCooldown;
    [SerializeField] private float dashStaminaRequirement;
    public float lastDashTime;
    private bool isDashing;
    private bool canDash = true;
    private enum MovementState {idle, jump, run, cIdle, cRun}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        standingCollSizeY = coll.size.y;
        standingCollOffsetY = coll.offset.y;
        crouchingCollSizeY = coll.size.y/2;
        crouchingCollOffsetY = coll.offset.y/2;
        sprite = GetComponent<SpriteRenderer>();
        staminaSystem = GetComponent<StaminaSystem>();
        trailRenderer = GetComponent<TrailRenderer>();
        lastDashTime = Time.time;
    }

    private void Update()
    {
        dirX = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded() && staminaSystem.currentStamina > jumpStaminaRequirement && !canDoWallJump && !wallJumping) {
            if (!hasNoRoom()) {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                ColliderStanding();
                staminaSystem.currentStamina -= jumpStaminaRequirement;
            }     
        }
        else if (Input.GetButtonDown("Jump") && staminaSystem.currentStamina > jumpStaminaRequirement && canDoWallJump)
        {
            wallJumping = true;
            Invoke("StopWallJump", wallJumpTime);
            if (!facingRight && wallJumping)
            {
                Debug.Log("Performed jump from LEFT wall.");
                //Invoke("StopWallJump", wallJumpTime);
                rb.velocity = new Vector2(wallJumpVelocity, jumpSpeed * 0.5f);
            }
            else if (facingRight && wallJumping)
            {
                Debug.Log("Performed jump from RIGHT wall.");
                //Invoke("StopWallJump", wallJumpTime);
                rb.velocity = new Vector2(-wallJumpVelocity, jumpSpeed * 0.5f);
            }
        }

        // add duration to wall jump so you jump for like 0.1sec in the other direction so you can change direction of what keys you press
        // (?) add so you can't stick to wall again for x time, or at all before dropping to ground
        // add so you can't stick to wall if you are close to ground (?)

        else if (Input.GetKey("s") && isGrounded()) {  
            rb.velocity = new Vector2(dirX * crouchSpeed, rb.velocity.y);
            ColliderCrouched();
        } 
        else {  
            if (hasNoRoom() && isGrounded()) rb.velocity = new Vector2(dirX * crouchSpeed, rb.velocity.y); 
            else {
                rb.velocity = new Vector2(dirX * walkSpeed, rb.velocity.y);
                ColliderStanding();
            }         
        }
        if(Input.GetKeyDown(KeyCode.C) && canDash && dashStaminaRequirement <= staminaSystem.currentStamina && Time.time - lastDashTime > dashCooldown)
        {
            isDashing = true;
            canDash = false;
            trailRenderer.emitting = true;
            lastDashTime = Time.time;
            staminaSystem.currentStamina -= dashStaminaRequirement;
            StartCoroutine(StopDashing());
        }

        if(isDashing)
        {
            rb.velocity = new Vector2(dirX * dashSpeed, rb.velocity.y);
        }

        if(isGrounded())
        {
            canDash = true;
            canStickToWall = true;
            canDoWallJump = true;
            wallJumping = false;
        }

        if (isTouchingWall() && /*!facingRight &&*/ !isGrounded() && rb.velocity.y <= 0f && canStickToWall && !isStickingToWall /*&& Time.time - wallStickTime > timeBeforeSlide*/)
        {
            canStickToWall = false;
            isStickingToWall = true;
            StartCoroutine(StickToWall());
            //rb.velocity = new Vector2(dirX, rb.velocity.y * wallSlideSpeedModifier);
        }

        if (isStickingToWall && !isGrounded() && isTouchingWall() && rb.velocity.y <= 0)
        {
            canDoWallJump = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        else if (!isGrounded() && isTouchingWall() && rb.velocity.y <=0 )
        {
            canDoWallJump = true;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            if(rb.velocity.y == 0f) { rb.velocity = new Vector2(dirX, -.1f); }
            rb.velocity = new Vector2(dirX, rb.velocity.y * wallSlideSpeedModifier);
        }else
        {
            canDoWallJump = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (rb.bodyType == RigidbodyType2D.Dynamic) AnimationUpdate();
    }
    
    private void AnimationUpdate()
    {
        MovementState state;

        if (isGrounded()) {
            if (isCrouched) {
                if (dirX != 0) state = MovementState.cRun;
                else state = MovementState.cIdle;
            }
            else if (dirX != 0) state = MovementState.run;
            else state = MovementState.idle;
        }
        else {        
            state = MovementState.jump;
        }

        if (dirX > 0f) {
            if (!facingRight) Flip();
        }
        else if (dirX < 0f) {
            if (facingRight) Flip();
        } 

        anim.SetInteger("state", (int)state);
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashTime);
        trailRenderer.emitting = false;
        isDashing = false;
    }

    private IEnumerator StickToWall()
    {
        yield return new WaitForSeconds(timeBeforeSlide);
        isStickingToWall = false;
    }

    private void ColliderStanding()
    {   
        isCrouched = false;
        coll.size = new Vector2(coll.size.x, standingCollSizeY);
        coll.offset = new Vector2(coll.offset.x, standingCollOffsetY);
    }

    private void ColliderCrouched() 
    {   
        isCrouched = true;
        coll.size = new Vector2(coll.size.x, crouchingCollSizeY);
        coll.offset = new Vector2(coll.offset.x, crouchingCollOffsetY);
    }

    private void Flip()
    {
        Vector2 currentScale = rb.transform.localScale;
        currentScale.x *= -1;
        rb.transform.localScale = currentScale;
        facingRight = !facingRight;
    } 

    private bool hasNoRoom()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.up, 0.125f, jumpableGround);
    }

    private bool isGrounded() 
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    private bool isTouchingWall()
    {
        return Physics2D.BoxCast(wallCheckPos.position, wallCheckSize, 0f, Vector2.left, 0f, jumpableGround);
    }

    private void StopWallJump()
    {
        wallJumping = false;
    }
}
