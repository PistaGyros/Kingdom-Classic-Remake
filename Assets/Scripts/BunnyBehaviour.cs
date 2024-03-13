using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyBehaviour : MonoBehaviour
{
    [SerializeField] GameObject coin;
    SpriteRenderer bunnySprite;
    [SerializeField] Sprite deadBunnySprite;

    // Start is called before the first frame update
    void Start()
    {
        bunnySprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
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
    }
}
