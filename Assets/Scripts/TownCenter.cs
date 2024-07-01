using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownCenter : MonoBehaviour
{
    private GameObject player;

    public int actualLvlOfCenter;

    private int requiredCoinsNextUp;
    private int requiredCoinsForUpToOne = 1;
    private int requiredCoinsForUpToTwo = 3;
    private int requiredCoinsForUpToThree = 6;
    private int requiredCoinsForUpToFour = 7;
    private int requiredCoinsForUpToFive = 8;
    private int requiredCoinsForUpToSix = 9;

    private bool playerHasCollided;
    private bool payButtonIsPressed;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    
    void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            playerHasCollided = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            playerHasCollided = false;
        }
    }
}
