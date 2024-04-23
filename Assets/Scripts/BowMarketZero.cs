using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowMarketZero : MonoBehaviour
{
    private SpriteRenderer spriteDrawer;

    [SerializeField] GameObject playerCharacter;
    [SerializeField] Sprite bowMarketZero;
    [SerializeField] Sprite bowMarketOne;
    [SerializeField] Sprite bowMarketTwo;
    [SerializeField] Sprite bowMarketThree;
    [SerializeField] Sprite bowMarketFour;

    private static string openBowMarket = "OpenBowMarket";
    private static string closedBowMarket = "BowMarket";

    private bool playerPayButtonIsPressed;
    private bool playerHasCollidedWithMarket;
    private int playerCoins;
    private int amountOfPaidCoins;
    private bool payingHasBeggun;
    private int actualLevelOfMarket;
    private int requiredCoinsForUpgrade = 2;

    public event EventHandler OnBowPickedUp;
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
                OnBowPickedUp?.Invoke(this, EventArgs.Empty);
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
            spriteDrawer.sprite = bowMarketZero;
            CancelInvoke("CallPeasent");
            OnStopCallPeasent?.Invoke(this, EventArgs.Empty);
            transform.tag = closedBowMarket;
        }
        else if (actualLevelOfMarket == 1)
        {
            spriteDrawer.sprite = bowMarketOne;
            transform.tag = openBowMarket;
        }
        else if (actualLevelOfMarket == 2)
            spriteDrawer.sprite = bowMarketTwo;
        else if (actualLevelOfMarket == 3)
            spriteDrawer.sprite = bowMarketThree;
        else if (actualLevelOfMarket == 4)
            spriteDrawer.sprite = bowMarketFour;
    }

    private void CallPeasent()
    {
        OnCallPeasent?.Invoke(this, EventArgs.Empty);
    }
}