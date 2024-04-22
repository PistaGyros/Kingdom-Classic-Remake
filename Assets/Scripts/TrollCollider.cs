using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollCollider : MonoBehaviour
{
    [SerializeField] private GameObject trollGrounder;
    private BoxCollider2D trollCollider2D;
    
    private LayerMask coinLayerMask;
    private LayerMask wallLayerMask;
    
    private int hitPoints = 1;

    private int direction;

    private Rigidbody2D rigidbody2D;


    private void Start()
    {
        rigidbody2D = GetComponentInParent<Rigidbody2D>();
        trollCollider2D = trollGrounder.GetComponent<BoxCollider2D>();
        coinLayerMask = LayerMask.GetMask("Coin");
        wallLayerMask = LayerMask.GetMask("Wall");
        if (transform.position.x > 0)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision2D)
    {
        if (collision2D.CompareTag("Arrow"))
        {
            hitPoints -= 1;
            if (hitPoints <= 0)
                Destroy(transform.parent.gameObject);
        }
        else if (collision2D.CompareTag("Wall") || collision2D.CompareTag("WallUnderAttack"))
        {
            // Debug.Log("Troll has collided");
            BackOff();
        }
        else if (collision2D.CompareTag("Ground"))
        {
            //rigidbody2D.velocity = new Vector2(0, 0);
        }
        else if (collision2D.CompareTag("EmptyWall"))
        {
            trollCollider2D.excludeLayers = wallLayerMask;
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("EmptyWall"))
        {
            trollCollider2D.excludeLayers = coinLayerMask;
        }
    }

    private void BackOff()
    {
        rigidbody2D.velocity = new Vector2(0, 0);
        rigidbody2D.velocity += new Vector2(-5f * direction, 1.5f);
    }
}