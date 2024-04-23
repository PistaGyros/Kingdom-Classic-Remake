using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Rendering;
using Random = System.Random;

public class BunnyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject coin;
    private SpriteRenderer bunnySprite;
    [SerializeField] private Sprite deadBunnySprite;
    private Rigidbody2D bunnyRigidbody2D;

    private int direction;
    private float bunnyWanderAgainTimer;
    private bool bunnyCanWanderAgain;
    private int[] possibleDirections = new int[4] {-1, 1, -1, 1};

    System.Random rnd = new System.Random();
    

    
    void Start()
    {
        bunnySprite = GetComponent<SpriteRenderer>();
        bunnyRigidbody2D = GetComponent<Rigidbody2D>();
        InvokeRepeating("BunnyWander", 1f, 8f);
    }

    
    void FixedUpdate()
    {
        BunnyMove();
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Arrow"))
        {
            gameObject.tag = "DeadBunny";
            Destroy(gameObject, 1f);
            bunnySprite.sprite = deadBunnySprite;
            Instantiate(coin, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        }
    }

    private void BunnyWander()
    {
        int r = rnd.Next(0, 3);
        direction = possibleDirections[r];
        FlipSprite();
        Invoke("BunnyStop", 0.5f);
    }

    private void BunnyMove()
    {
        Vector2 bunnyVelocity = new Vector2(direction, 0);
        bunnyRigidbody2D.velocity = bunnyVelocity * 1;
    }

    private void BunnyStop()
    {
        direction = 0;
    }

    private void FlipSprite()
    {
        transform.localScale = new Vector2(direction, 1);
    }
}
