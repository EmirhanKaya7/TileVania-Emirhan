using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    
    [SerializeField] float moveSpeed = 1f; 
    Rigidbody2D myrigid;
    void Start()
    {
        myrigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        myrigid.velocity = new Vector2(moveSpeed, 0);
    }
    void OnTriggerExit2D(Collider2D other){
    moveSpeed = -moveSpeed;
    FlipEnemyFacing();
    }
    void FlipEnemyFacing(){
        transform.localScale = new Vector2 (-(Mathf.Sign(myrigid.velocity.x)),1f);
    }


}
