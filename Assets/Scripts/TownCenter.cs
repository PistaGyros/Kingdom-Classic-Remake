using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownCenter : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private GameObject bg;
    private SpriteRenderer bgSpriteRenderer;
    [SerializeField] private GameObject campFire;
    private Animator campAnimator;
    [SerializeField] private GameObject fortification;
    private SpriteRenderer fortificationRenderer;
    
    [SerializeField] Sprite[] bgSprites;
    [SerializeField] private Sprite[] fortificationSprites;

    public int actualLvlOfCenter = 0;
    private static string upgradableTC = "UpgradableTC";
    private static string TC = "TC";

    private int requiredCoinsNextUp;
    private int[] requiredCoinsForUp = new int[7] { 0, 1, 3, 6, 7, 8, 9 };
    private Vector2[] posOfBg = new Vector2[7]
    {
        new Vector2(),
        new Vector2(),
        new Vector2(0.62f, 0.72f),
        new Vector2(0.62f, 0.72f),
        new Vector2(0, 2f),
        new Vector2(0, 2f),
        new Vector2(0, 2.35f)
    };

    private bool playerHasCollided;
    private bool payButtonIsPressed;
    private bool payingHasBeggun = false;
    private bool TCCanBeUp = true;
    private int amountOfPaidCoins;

    public event EventHandler<TownCenterArgs> OnTownCenterUpgrade;

    public class TownCenterArgs : EventArgs
    {
        public int actualLvlOfCenter;
    }

    void Start()
    {
        player = GameObject.Find("Player");
        bgSpriteRenderer = bg.GetComponent<SpriteRenderer>();
        bgSpriteRenderer.sprite = bgSprites[0];
        campAnimator = campFire.GetComponent<Animator>();
        fortificationRenderer = fortification.GetComponent<SpriteRenderer>();
    }

    
    void FixedUpdate()
    {
        payButtonIsPressed = player.GetComponent<PickDropCoins>().payButtonIsPressed;

        if (playerHasCollided)
        {
            if (!payingHasBeggun && payButtonIsPressed && TCCanBeUp && actualLvlOfCenter <= 5)
            {
                payingHasBeggun = true;
                InvokeRepeating("PayToTownCenter", 0.2f, 0.5f);
            }
        }
        else if (!playerHasCollided || !payButtonIsPressed)
        {
            payingHasBeggun = false;
            CancelInvoke("PayToTownCenter");
        }
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

    private void PayToTownCenter()
    {
        int playerCoins = player.GetComponent<PickDropCoins>().numberOfCoins;
        if (TCCanBeUp && playerCoins >= 1)
        {
            player.GetComponent<PickDropCoins>().numberOfCoins--;
            amountOfPaidCoins++;
            Debug.Log("Coin has been paid to TC, amount if paid coins: " + amountOfPaidCoins);
            if (amountOfPaidCoins == requiredCoinsForUp[actualLvlOfCenter + 1])
            {
                // succesful payment
                Debug.Log("Succesful payment");
                TCCanBeUp = false;
                // change delay time to 120s
                Invoke("DelayOfNextPayment", 2f);
                transform.tag = TC;
                payingHasBeggun = false;
                amountOfPaidCoins = 0;
                actualLvlOfCenter++;
                UpgradeCenter();
                CancelInvoke("PayToTownCenter");
                OnTownCenterUpgrade?.Invoke(this, new TownCenterArgs{actualLvlOfCenter = actualLvlOfCenter});
            }
        }
        else
            CancelInvoke("PayToTownCenter");
    }

    private void UpgradeCenter()
    {
        if (actualLvlOfCenter == 1)
        {
            // campfire has been lit, let the king rule
            campFire.transform.position = new Vector2(0, 1.42f);
            campAnimator.SetBool("campIsLit", true);
        }
        else
        {
            bgSpriteRenderer.sprite = bgSprites[actualLvlOfCenter];
            bg.transform.position = posOfBg[actualLvlOfCenter];
            fortificationRenderer.sprite = fortificationSprites[actualLvlOfCenter];
        }
    }

    private void DelayOfNextPayment()
    {
        TCCanBeUp = true;
        transform.tag = upgradableTC;
    }
}
