 using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cinemachine;
using System;
using System.Security.Cryptography;
using DG.Tweening;
public class PlayerMovement : MonoBehaviour
{

   
   

    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathkick = new Vector2(10f,10f);
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject coinNumPre;

    [SerializeField] Transform gun;

    [SerializeField] CinemachineVirtualCamera fpcamera;
    [SerializeField] CinemachineVirtualCamera tpcamera;


    Vector2 moveInput;
    Rigidbody2D myRigid;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    bool isAlive = true;
    bool canDoubleJump;
    float gravityScaleAtStart;

    public int coins;
    
    public CoinCollect coinCollect;


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
        coinCollect = FindObjectOfType<CoinCollect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive){return;}
        
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
        //SwitchCam();
        
    }

   /*void SwitchCam(){
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
   */

    void OnMove(InputValue value) {
        if (!isAlive){return;}
        moveInput = value.Get<Vector2>();
    }
    
    void OnJump(InputValue value)
    {
        if (!isAlive){return;}
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
    void OnFire(InputValue value)
    {
        if(!isAlive) { return;}
        bullet.GetComponent<SpriteRenderer>().DOColor(UnityEngine.Random.ColorHSV(),1f);
        Instantiate(bullet,gun.position,transform.rotation);
        
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

    void Die(){
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying"); 
            myRigid.velocity = deathkick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
            
        }
    }
    bool wasCollected = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Coin")&&!wasCollected)
        {
            coinCollect.AddCoins(other.transform.position,2);
            
            Destroy(other.gameObject);
            
            //Destroy(Instantiate(coinNumPre,other.transform.position,Quaternion.identity),1f);
        }
    }
}
