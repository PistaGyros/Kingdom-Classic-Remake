using UnityEngine;
using UnityEngine.UIElements;

public class BeggarPickUpCoin : MonoBehaviour
{
    float beggarSpeed = 5f;
    Rigidbody2D rigidbody2D;
    float direction;
    [SerializeField] GameObject peasent;
    



    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        RunForCoin();
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Coins") || collider2D.gameObject.CompareTag("PlayerCoins"))
        {
            direction = 0;
            Destroy(gameObject, 0f);
            Instantiate(peasent, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);            
        }
    }

    private void OnTriggerStay2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("CoinCall") || collider2D.gameObject.CompareTag("PlayerCoinCall"))
        {
            //It checks if beggar is on the righ or on the left site of the coin or player coin
            if (transform.position.x > collider2D.transform.position.x)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("CoinCall") || collider2D.gameObject.CompareTag("PlayerCoinCall"))
        {
            direction = 0;
        }
    }

    void RunForCoin()
    {
        Vector2 beggarVelocity = new Vector2(direction, 0);
        rigidbody2D.velocity = beggarVelocity * beggarSpeed;
    }
}