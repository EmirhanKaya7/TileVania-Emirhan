using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    Vector2 moveInput;
    Rigidbody2D myRigid;
    Animator myAnimator;
    CapsuleCollider2D myCapsule;
    bool canDoubleJump;
    float gravityScaleAtStart;
    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsule = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = myRigid.gravityScale;

    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (!myCapsule.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            if (canDoubleJump)
            {
                myRigid.velocity += new Vector2(0f, jumpSpeed);
                canDoubleJump=false;
            }
            return; 
        }
        if (value.isPressed)
        {
            myRigid.velocity += new Vector2(0f, jumpSpeed);
            canDoubleJump=true;

        }

    }
    
    void Run(){
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed,myRigid.velocity.y);
        myRigid.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigid.velocity.x)> Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite(){
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigid.velocity.x)> Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
        transform.localScale = new Vector2(Mathf.Sign(myRigid.velocity.x), 1f);    
        }

        
    }

    void ClimbLadder(){

        if (!myCapsule.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
           myRigid.gravityScale = gravityScaleAtStart;
           myAnimator.SetBool("isClimbing",false); 
            return; 
        }
        Vector2 ClimbVelocity = new Vector2(myRigid.velocity.x, moveInput.y * climbSpeed);
        myRigid.velocity = ClimbVelocity;
        myRigid.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigid.velocity.y)> Mathf.Epsilon;
        myAnimator.SetBool("isClimbing",playerHasVerticalSpeed); 

    }
}