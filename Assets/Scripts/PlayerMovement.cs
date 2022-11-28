using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{



    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    [SerializeField] CinemachineVirtualCamera fpcamera;
    [SerializeField] CinemachineVirtualCamera tpcamera;


    Vector2 moveInput;
    Rigidbody2D myRigid;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    bool canDoubleJump;
    float gravityScaleAtStart;

    public bool ChangeCamera {get; private set;} = false;
    // Start is called before the first frame update

    private void OnEnable(){
        CameraScript.Register(tpcamera);
        CameraScript.Register(fpcamera);
        CameraScript.SwitchCam(tpcamera);
    }
private void OnDisable(){
        CameraScript.Unregister(tpcamera);
        CameraScript.Unregister(fpcamera);

}
    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigid.gravityScale;

    }

    // Update is called once per frame
    void Update()
    {
        
        
        Run();
        FlipSprite();
        ClimbLadder();

        if (Input.GetKey(KeyCode.Q))
        {
            if (CameraScript.isActiveCam(tpcamera))
            {
                CameraScript.SwitchCam(fpcamera);
            }else if (CameraScript.isActiveCam(fpcamera))
            {
                CameraScript.SwitchCam(tpcamera);
            }
            
        }
    }

    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
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

        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
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
