using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Coin : MonoBehaviour
{
    Collider2D coinCollider;
    private Rigidbody2D coinRigidbody2D;
    private int[] oneAndMinusOne = new int[4] {-1, -1, 1, 1};
    private Random rnd = new Random();
    

    void Start()
    {
        coinCollider = GetComponentInParent<Collider2D>();
        coinRigidbody2D = GetComponentInParent<Rigidbody2D>();
        int r = rnd.Next(0, 3);
        coinRigidbody2D.AddForce(new Vector2(oneAndMinusOne[r] * 75, 100));
    }

    
    void FixedUpdate()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Player") || collider2D.CompareTag("Beggar") || collider2D.CompareTag("Peasents")
            || collider2D.CompareTag("Builder") || collider2D.CompareTag("Archer") || collider2D.CompareTag("Farmer"))
        {
            coinCollider.enabled = false;
            Destroy(gameObject, 0f);
            Destroy(transform.parent.gameObject, 1f);
        }
    }
}