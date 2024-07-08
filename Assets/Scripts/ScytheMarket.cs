using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScytheMarket : MonoBehaviour
{
    private SpriteRenderer spriteDrawer;

    [SerializeField] GameObject playerCharacter;
    [SerializeField] private GameObject townCenter;
    private BoxCollider2D marketCollider;

    [SerializeField] private GameObject outliner;
    private SpriteRenderer outlineRenderer;
    
    [SerializeField] Sprite scytheMarketZero;
    [SerializeField] Sprite scytheMarketOne;
    [SerializeField] Sprite scytheMarketTwo;
    [SerializeField] Sprite scytheMarketThree;
    [SerializeField] Sprite scytheMarketFour;

    private static string openScytheMarket = "OpenScytheMarket";
    private static string closedScytheMarket = "ScytheMarket";

    private bool playerPayButtonIsPressed;
    private bool playerHasCollidedWithMarket;
    private int playerCoins;
    private int amountOfPaidCoins;
    private bool payingHasBeggun;
    private int actualLevelOfMarket;
    private int requiredCoinsForUpgrade = 5;

    public event EventHandler OnCallPeasent;
    public event EventHandler OnStopCallPeasent;


    void Start()
    {
        playerCharacter = GameObject.Find("Player");
        spriteDrawer = GetComponent<SpriteRenderer>();
        spriteDrawer.enabled = false;
        marketCollider = GetComponent<BoxCollider2D>();
        marketCollider.enabled = false;
        TownCenter tc = townCenter.GetComponent<TownCenter>();
        tc.OnTownCenterUpgrade += TcOnOnTownCenterUpgrade;
        outlineRenderer = outliner.GetComponent<SpriteRenderer>();
    }

    private void TcOnOnTownCenterUpgrade(object sender, TownCenter.TownCenterArgs e)
    {
        if (e.actualLvlOfCenter >= 3)
        {
            spriteDrawer.enabled = true;
            marketCollider.enabled = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerHasCollidedWithMarket)
        {
            playerCoins = playerCharacter.GetComponent<PickDropCoins>().numberOfCoins;
            playerPayButtonIsPressed = playerCharacter.GetComponent<PickDropCoins>().payButtonIsPressed;

            if (playerPayButtonIsPressed && !payingHasBeggun)
            {
                if (actualLevelOfMarket <= 3)
                {
                    payingHasBeggun = true;
                    InvokeRepeating("PayToMarket", 0.2f, 0.5f);
                }
            }
            else if (!playerPayButtonIsPressed || !playerHasCollidedWithMarket)
            {
                payingHasBeggun = false;
                CancelInvoke("PayToMarket");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            playerHasCollidedWithMarket = true;
            outlineRenderer.enabled = true;
        }
        if (collider2D.CompareTag("Peasents"))
        {
            if (actualLevelOfMarket >= 1)
            {
                actualLevelOfMarket--;
                ChangerToAnotherLvl();
                Debug.Log("Peasent has picked up a tool, lvl of market has changed to " + actualLevelOfMarket);
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
            outlineRenderer.enabled = false;
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
            spriteDrawer.sprite = scytheMarketZero;
            CancelInvoke("CallPeasent");
            if (OnStopCallPeasent != null)
                OnStopCallPeasent(this, EventArgs.Empty);
            transform.tag = closedScytheMarket;
        }
        else if (actualLevelOfMarket == 1)
        {
            spriteDrawer.sprite = scytheMarketOne;
            transform.tag = openScytheMarket;
        }
        else if (actualLevelOfMarket == 2)
            spriteDrawer.sprite = scytheMarketTwo;
        else if (actualLevelOfMarket == 3)
            spriteDrawer.sprite = scytheMarketThree;
        else if (actualLevelOfMarket == 4)
            spriteDrawer.sprite = scytheMarketFour;
    }

    private void CallPeasent()
    {
        if (OnCallPeasent != null)
            OnCallPeasent(this, EventArgs.Empty);
    }
}
