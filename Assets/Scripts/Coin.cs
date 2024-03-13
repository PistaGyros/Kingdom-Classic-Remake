using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Collider2D coinCollider;

    void Start()
    {
        coinCollider = GetComponentInParent<Collider2D>();
    }

    
    void Update()
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