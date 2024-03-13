using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCoin : MonoBehaviour
{
    Collider2D playerCoinCollider;

    void Start()
    {
        playerCoinCollider = GetComponentInParent<Collider2D>();
        gameObject.tag = "PlayerCoins";
    }
    
    void Update()
    {
        
    }


    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if ((collider2D.CompareTag("Player") && gameObject.tag == "Coins") || collider2D.gameObject.CompareTag("Beggar") || collider2D.gameObject.CompareTag("Peasents") 
            || collider2D.CompareTag("Archer") || collider2D.CompareTag("Builder"))
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