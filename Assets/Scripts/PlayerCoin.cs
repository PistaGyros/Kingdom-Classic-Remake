using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class PlayerCoin : MonoBehaviour
{
    Collider2D playerCoinCollider;
    private Rigidbody2D rigidbody2D;
    
    private int[] oneAndMinusOne = new int[4] {-1, -1, 1, 1};
    private Random rnd = new Random();

    void Start()
    {
        playerCoinCollider = GetComponentInParent<Collider2D>();
        gameObject.tag = "PlayerCoins";
        rigidbody2D = GetComponentInParent<Rigidbody2D>();
        int r = rnd.Next(0, 3);
        rigidbody2D.AddForce(new Vector2(oneAndMinusOne[r] * 5, 30));
    }
    
    void FixedUpdate()
    {
        
    }


    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if ((collider2D.CompareTag("Player") && gameObject.tag == "Coins") || collider2D.gameObject.CompareTag("Beggar") 
            || collider2D.gameObject.CompareTag("Peasents") || collider2D.CompareTag("Archer") || 
            collider2D.CompareTag("Builder"))
        {
            playerCoinCollider.enabled = false;
            Destroy(gameObject, 0f);
            Destroy(transform.parent.gameObject, 1f);
        }
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            gameObject.tag = "Coins";
        }
    }
}