using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Rendering;
using Random = System.Random;

public class BunnyBehaviour : MonoBehaviour
{
    [SerializeField] GameObject coin;
    SpriteRenderer bunnySprite;
    [SerializeField] Sprite deadBunnySprite;
    private Rigidbody2D bunnyRigidbody2D;

    private Vector2 possibleLeftPos;
    private Vector2 possibleRightPos;
    private Vector2 actualPos;
    private int direction;
    private float bunnyWanderAgainTimer;
    private bool bunnyCanWanderAgain;
    private int[] possibleDirections = new int[4] {-1, -1, 1, 1};

    System.Random rnd = new System.Random();
    

    
    void Start()
    {
        bunnySprite = GetComponent<SpriteRenderer>();
        bunnyRigidbody2D = GetComponent<Rigidbody2D>();
        
    }

    
    void FixedUpdate()
    {
        bunnyWanderAgainTimer -= Time.fixedDeltaTime;
        if (bunnyWanderAgainTimer <= 0)
        {
            BunnyWander();
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Arrow"))
        {
            Destroy(gameObject, 1f);
            bunnySprite.sprite = deadBunnySprite;
            Instantiate(coin, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        }
        else if (collider2D.CompareTag("Ground"))
        {
            Vector2 colliderPos = collider2D.transform.position;
            possibleLeftPos = new Vector2(colliderPos.x - 2, 0.15f);
            possibleRightPos = new Vector2(colliderPos.x + 2, 0.15f);
        }
    }

    private void BunnyWander()
    {
        if (bunnyCanWanderAgain)
        {
            bunnyCanWanderAgain = false;
            Invoke("SetBunnyWanderAgainTimer", 0.5f);
            if (transform.position.x <= possibleLeftPos.x)
            {
                direction = 1;
            }
            else if (transform.position.x >= possibleRightPos.x)
            {
                direction = -1;
            }
            else
            {
                int r = rnd.Next(0, 3);
                direction = possibleDirections[r];
            }
        }
        Vector2 bunnyVelocity = new Vector2(direction, 0);
        bunnyRigidbody2D.velocity = bunnyVelocity * 5;
    }

    private void SetBunnyWanderAgainTimer()
    {
        direction = 0;
        bunnyCanWanderAgain = true;
        bunnyWanderAgainTimer = 5f;
    }
}
