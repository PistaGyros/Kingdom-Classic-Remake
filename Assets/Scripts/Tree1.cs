using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree1 : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] Sprite unMarkedSprite;
    [SerializeField] Sprite markedSprite;
    [SerializeField] Sprite treeWOutline;
    [SerializeField] GameObject coin;
    private GameObject player;

    [SerializeField] private GameObject outliner;
    // 
    private SpriteRenderer outlineRenderer;

    private float cutDownTime = 0f;
    private float progresCutDownTime = 0f;
    private bool treeIsUnderConstruction = false;
    private bool treeIsMarked = false;

    private bool playerPayButtonIsPressed;
    private bool playerHasCollidedWithTree;
    private int playerCoins;
    private int amountOfPaidCoins;
    private bool payingHasBeggun;
    private int requiredCoinsForMark = 1;

    public event EventHandler <CallToBuilderArgs> OnCallToBuildersToTree;
    public event EventHandler OnStopCallToBuildersToTree;

    public class CallToBuilderArgs : EventArgs
    {
        public Vector2 positionOfTree;
    }


    void Start()
    {
        player = GameObject.Find("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        outlineRenderer = outliner.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        playerPayButtonIsPressed = player.GetComponent<PickDropCoins>().payButtonIsPressed;

        if (playerHasCollidedWithTree && playerPayButtonIsPressed && !payingHasBeggun)
        {
            payingHasBeggun = true;
            StartInvoke();
        }
        else if (!playerPayButtonIsPressed || !playerHasCollidedWithTree)
        {
            CancelInvoke("PayToTree");
            payingHasBeggun = false;
        }
        if (cutDownTime > 0 && treeIsUnderConstruction)
            cutDownTime -= Time.fixedDeltaTime;
        else if (cutDownTime < 0 && treeIsUnderConstruction)
        {
            treeIsUnderConstruction = false;
            SpawnCoin();
            Destroy(gameObject, 1f);
            CancelInvoke("CallBuilder");
            OnStopCallToBuildersToTree?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            if (!treeIsMarked)
            {
                playerHasCollidedWithTree = true;
                outlineRenderer.enabled = true;
            }            
        }

        if (collider2D.CompareTag("Builder"))
        {
            if (treeIsMarked)
            {
                treeIsUnderConstruction = true;
                if (progresCutDownTime == 0)
                    cutDownTime = 4f;
                else
                    cutDownTime = progresCutDownTime;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            playerHasCollidedWithTree = false;
            outlineRenderer.enabled = false;
        }

        if (collider2D.CompareTag("Builder"))
        {
            treeIsUnderConstruction = false;
            progresCutDownTime = cutDownTime;
        }
    }

    private void StartInvoke()
    {
        InvokeRepeating("PayToTree", 0.2f, 0.5f);
    }


    private void SpawnCoin()
    {
        Instantiate(coin, transform.position, Quaternion.identity);
    }

    private void PayToTree()
    {
        playerCoins = player.GetComponent<PickDropCoins>().numberOfCoins;
        if (playerCoins >= requiredCoinsForMark && !treeIsMarked)
        {
            player.GetComponent<PickDropCoins>().numberOfCoins--;
            amountOfPaidCoins++;
            if (amountOfPaidCoins >= requiredCoinsForMark)
            {
                treeIsMarked = true;
                amountOfPaidCoins = 0;
                CancelInvoke("PayToTree");
                ChangeToMarkTree();
                InvokeRepeating("CallBuilder", 0f, 5f);
                Debug.Log("Tree has been market");
            }
        }        
    }

    private void ChangeToMarkTree()
    {
        spriteRenderer.sprite = markedSprite;
        transform.tag = "MarkedTree";
    }

    private void CallBuilder()
    {
        OnCallToBuildersToTree?.Invoke(this, new CallToBuilderArgs { positionOfTree = transform.position });
        Debug.Log("Builder has been called");
    }
}
