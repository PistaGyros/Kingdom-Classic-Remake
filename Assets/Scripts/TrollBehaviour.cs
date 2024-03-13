using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollBehaviour : MonoBehaviour
{
    [SerializeField] GameObject trollCollider;

    private Rigidbody2D rigidbody2D;

    private bool run;
    private float canJumpAgain = 0;
    private bool canAttack = false;

    private int direction;
    private int trollSpeed = 6;

    private Vector2 startJumpCords;
    private Vector2 startPosition;


    void Start()
    {
        rigidbody2D = GetComponentInParent<Rigidbody2D>();
        FlipSprite();
        startPosition = transform.position;        
    }


    private void FixedUpdate()
    {
        Run();
        Attack();
        canJumpAgain -= Time.fixedDeltaTime;
        //Debug.Log(canJumpAgain);
    }


    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Wall") || collider2D.CompareTag("WallUnderAttack"))
        {
            run = false;
            canAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("WallUnderAttack") || collider2D.CompareTag("Wall"))
        {
            canAttack = false;
            run = true;            
        }
    }


    private void Attack() 
    {
        if (canAttack == true && canJumpAgain <= 0)
        {
            canJumpAgain = 1f;
            rigidbody2D.velocity += new Vector2(14f * direction, 14f);
        }
    }

    

    private void Run()
    {
        if (run)
        {
            Vector2 trollVelocity = new Vector2(direction, 0);
            rigidbody2D.velocity = trollVelocity * trollSpeed;
        }
    }


    private void FlipSprite()
    {
        if (transform.position.x > 0)
        {
            transform.parent.localScale = new Vector2(-1f, 1f);
            run = true;
            direction = -1;
        }
        else
        {
            transform.parent.localScale = new Vector2(1f, 1f);
            run = true;
            direction = 1;
        }
        
    }
}