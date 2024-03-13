using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehaviour : MonoBehaviour
{
    [SerializeField] GameObject coin;
    int numberOfCoins = 5;
    bool canPickUp;


    void Start()
    {
        canPickUp = true;
    }

    
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            canPickUp = false;
            Invoke("TurnCanPickUpBackToTrue", 2f);
        }
        else if (collider2D.CompareTag("PlayerCoins") || collider2D.CompareTag("Coins"))
        {
            PickUpCoin();
        }
        
    }

    private void OnTriggerStay2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            DropCoin();
            Invoke("DropCoin", 0.5f);
        }

    }

    private void PickUpCoin()
    {
        if (canPickUp)
        {
            numberOfCoins++;
        }        
    }

    private void TurnCanPickUpBackToTrue()
    {
        canPickUp = true;
    }

    private void DropCoin()
    {
        if(numberOfCoins > 0)
        {
            numberOfCoins--;
            Instantiate(coin, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        }        
    }
}