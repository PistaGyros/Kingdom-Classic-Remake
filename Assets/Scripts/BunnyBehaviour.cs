using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyBehaviour : MonoBehaviour
{
    [SerializeField] GameObject coin;
    SpriteRenderer bunnySprite;
    [SerializeField] Sprite deadBunnySprite;
    private Rigidbody2D bunnyRigidbody2D;

    
    void Start()
    {
        bunnySprite = GetComponent<SpriteRenderer>();
        bunnyRigidbody2D = GetComponent<Rigidbody2D>();
    }

    
    void FixedUpdate()
    {
        
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
            
        }
    }
}
