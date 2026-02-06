using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;

public class PickDropCoins : MonoBehaviour
{
    

    public int numberOfCoins = 100;
    public GameObject coin;
    
    bool isCollidingWithMarket;
    public bool payButtonIsPressed;
    bool isCollidingWithTree;
    bool isCollidingWithUpgradableWall;
    private bool isCollidingWwithTownCenter;
    private bool isCollidingWithTower;
    private bool isCollidingWithFarm;


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
        if (collider2D.CompareTag("BowMarket") || collider2D.CompareTag("HammerMarket") || collider2D.CompareTag("OpenBowMarket") || collider2D.CompareTag("OpenHammerMarket")
            || collider2D.CompareTag("OpenScytheMarket") || collider2D.CompareTag("ScytheMarket"))
        {
            isCollidingWithMarket = true;
        }
        else if (collider2D.CompareTag("UpgradableTC"))
            isCollidingWwithTownCenter = true;
        else if (collider2D.CompareTag("TC"))
            isCollidingWwithTownCenter = false;
        else if (collider2D.CompareTag("UpgradableTower"))
            isCollidingWithTower = true;
        else if (collider2D.CompareTag("TowerUnderConstruct"))
            isCollidingWithTower = false;
        else if (collider2D.CompareTag("MarkableTrees"))
        {
            isCollidingWithTree = true;
        }
        else if (collider2D.CompareTag("Wall") || collider2D.CompareTag("WallUnderAttack") || 
                 collider2D.CompareTag("MarkedWall") || collider2D.CompareTag("EmptyWall"))
        {
            isCollidingWithUpgradableWall = true;
        }
        else if (collider2D.CompareTag("OpenFarm"))
        {
            isCollidingWithFarm = true;
        }
    }


    void OnTriggerExit2D(Collider2D collider2D)
    {
        if(collider2D.CompareTag("BowMarket") || collider2D.CompareTag("HammerMarket") || collider2D.CompareTag("OpenBowMarket") || collider2D.CompareTag("OpenHammerMarket")
            || collider2D.CompareTag("OpenScytheMarket") || collider2D.CompareTag("ScytheMarket"))
        {
            isCollidingWithMarket = false;
        }
        else if (collider2D.CompareTag("UpgradableTC"))
            isCollidingWwithTownCenter = false;
        else if (collider2D.CompareTag("TC"))
            isCollidingWwithTownCenter = false;
        else if (collider2D.CompareTag("UpgradableTower"))
            isCollidingWithTower = false;
        else if (collider2D.CompareTag("TowerUnderConstruct"))
            isCollidingWithTower = false;
        else if (collider2D.CompareTag("MarkableTrees") || collider2D.CompareTag("MarkedTree"))
        {
            isCollidingWithTree = false;
        }
        else if (collider2D.CompareTag("Wall") || collider2D.CompareTag("WallUnderAttack") || 
                 collider2D.CompareTag("MarkedWall") || collider2D.CompareTag("EmptyWall"))
        {
            isCollidingWithUpgradableWall = false;
        }
        else if (collider2D.CompareTag("OpenFarm"))
        {
            isCollidingWithFarm = false;
        }
    }


    void OnDropCoin(InputValue value)
    {
        if (numberOfCoins > 0 && !isCollidingWithMarket && !isCollidingWithTree && !isCollidingWithUpgradableWall && 
            !isCollidingWwithTownCenter && !isCollidingWithTower && !isCollidingWithFarm)
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