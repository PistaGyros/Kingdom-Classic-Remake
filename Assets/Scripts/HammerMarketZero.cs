using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HammerMarketZero : MonoBehaviour
{
    private SpriteRenderer spriteDrawer;

    [SerializeField] GameObject playerCharacter;
    [SerializeField] Sprite hammerMarketZero;
    [SerializeField] Sprite hammerMarketOne;
    [SerializeField] Sprite hammerMarketTwo;
    [SerializeField] Sprite hammerMarketThree;
    [SerializeField] Sprite hammerMarketFour;

    private static string closedMarketTag = "HammerMarket";
    private static string openMarketTag = "OpenHammerMarket";

    private bool playerPayButtonIsPressed;
    private bool playerHasCollidedWithMarket;
    private int playerCoins;
    private int amountOfPaidCoins;
    private bool payingHasBeggun;
    private int actualLevelOfMarket;
    private int requiredCoinsForUpgrade = 3;

    public event EventHandler OnCallPeasent;
    public event EventHandler OnStopCallPeasent;

    void Start()
    {
        playerCharacter = GameObject.Find("Player");
        spriteDrawer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        playerCoins = playerCharacter.GetComponent<PickDropCoins>().numberOfCoins;
        playerPayButtonIsPressed = playerCharacter.GetComponent<PickDropCoins>().payButtonIsPressed;

        if (playerHasCollidedWithMarket && playerPayButtonIsPressed && !payingHasBeggun)
        {
            if (actualLevelOfMarket <= 3)
            {
                payingHasBeggun = true;
                StartInvoke();
            }
        }
        else if (!playerPayButtonIsPressed || !playerHasCollidedWithMarket)
        {
            CancelInvoke("PayToMarket");
            payingHasBeggun = false;
        }
    }

    private void StartInvoke()
    {
        InvokeRepeating("PayToMarket", 0.2f, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            playerHasCollidedWithMarket = true;
        }
        if (collider2D.CompareTag("Peasents"))
        {
            if (actualLevelOfMarket >= 1)
            {
                actualLevelOfMarket--;
                ChangerToAnotherLvl();
                //Debug.Log("Peasent has picked up a tool, lvl of market has changed to " + actualLevelOfMarket);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            playerHasCollidedWithMarket = false;
            amountOfPaidCoins = 0;
            CancelInvoke("PayToMarket");
        }
    }

    private void PayToMarket()
    {
        if (playerCharacter.GetComponent<PickDropCoins>().numberOfCoins >= requiredCoinsForUpgrade)
        {
            playerCharacter.GetComponent<PickDropCoins>().numberOfCoins--;
            amountOfPaidCoins++;
            Debug.Log(playerCoins);
            Debug.Log(amountOfPaidCoins);
            if (amountOfPaidCoins >= requiredCoinsForUpgrade)
            {
                amountOfPaidCoins = 0;
                CancelInvoke("PayToMarket");
                actualLevelOfMarket++;
                ChangerToAnotherLvl();
                InvokeRepeating("CallPeasent", 0f, 5f);
                Debug.Log("Market has been upgraded to " + actualLevelOfMarket + " lvl");
            }
        }        
    }

    private void ChangerToAnotherLvl()
    {
        if (actualLevelOfMarket == 0)
        {
            spriteDrawer.sprite = hammerMarketZero;
            CancelInvoke("CallPeasent");
            if (OnStopCallPeasent != null)
                OnStopCallPeasent(this, EventArgs.Empty);
            transform.tag = closedMarketTag;
        }
        else if (actualLevelOfMarket == 1)
        {
            spriteDrawer.sprite = hammerMarketOne;
            transform.tag = openMarketTag;
        }
        else if (actualLevelOfMarket == 2)
            spriteDrawer.sprite = hammerMarketTwo;
        else if (actualLevelOfMarket == 3)
            spriteDrawer.sprite = hammerMarketThree;
        else if (actualLevelOfMarket == 4)
            spriteDrawer.sprite = hammerMarketFour;
    }

    private void CallPeasent()
    {
        if (OnCallPeasent != null)
            OnCallPeasent(this, EventArgs.Empty);
    }
}