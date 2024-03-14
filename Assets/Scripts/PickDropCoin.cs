using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;

public class PickDropCoins : MonoBehaviour
{
    

    public int numberOfCoins = 100;
    public GameObject coin;
    [SerializeField] GameObject scaffold0;
    [SerializeField] GameObject scaffold1;

    bool isCollidingWithMarket;
    public bool payButtonIsPressed;
    bool isCollidingWithTree;
    bool isCollidingWithUpgradableWall;


    void Start()
    {

    }


    void Update()
    {
        
    }


    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Coins"))
        {
            numberOfCoins += 1;
        }
        if (collider2D.CompareTag("BowMarket") || collider2D.CompareTag("HammerMarket") || collider2D.CompareTag("OpenBowMarket") || collider2D.CompareTag("OpenHammerMarket"))
        {
            isCollidingWithMarket = true;
        }
        else if (collider2D.CompareTag("MarkableTrees"))
        {
            isCollidingWithTree = true;
        }
        else if (collider2D.CompareTag("Wall") || collider2D.CompareTag("WallUnderAttack") || 
                 collider2D.CompareTag("MarkedWall") || collider2D.CompareTag("EmptyWall"))
        {
            isCollidingWithUpgradableWall = true;
        }
    }


    void OnTriggerExit2D(Collider2D collider2D)
    {
        if(collider2D.CompareTag("BowMarket") || collider2D.CompareTag("HammerMarket") || collider2D.CompareTag("OpenBowMarket") || collider2D.CompareTag("OpenHammerMarket"))
        {
            isCollidingWithMarket = false;
        }
        else if (collider2D.CompareTag("MarkableTrees") || collider2D.CompareTag("MarkedTree"))
        {
            isCollidingWithTree = false;
        }
        else if (collider2D.CompareTag("Wall") || collider2D.CompareTag("WallUnderAttack") || 
                 collider2D.CompareTag("MarkedWall") || collider2D.CompareTag("EmptyWall"))
        {
            isCollidingWithUpgradableWall = false;
        }
    }


    void OnDropCoin(InputValue value)
    {
        if (numberOfCoins > 0 && !isCollidingWithMarket && !isCollidingWithTree && !isCollidingWithUpgradableWall)
        {
            numberOfCoins -= 1;
            SpawnCoin();
        }
        
    }

    void OnPayWithCoin(InputValue value)
    {
        payButtonIsPressed = true;
    }

    void OnDontPayWithCoin(InputValue value)
    {
        payButtonIsPressed = false;
    }


    void SpawnCoin()
    {        
        Instantiate(coin, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
    }
}