using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigid;


    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
    }

    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }
    
    void Run(){
        myRigid.velocity = moveInput;
    }

}
