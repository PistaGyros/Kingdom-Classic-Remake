using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class Wall : MonoBehaviour
{
    [SerializeField] private GameObject scaffold;
    private GameObject playerCharacter;
    private List<GameObject> assignedBuilders = new List<GameObject>();
    private BoxCollider2D boxCollider2D;
    private GameObject townCenter;

    [SerializeField] private Sprite baseForWall;
    [SerializeField] private Sprite wallLvlOne;
    [SerializeField] private Sprite wallLvlOneBroke;
    [SerializeField] private Sprite wallLvlTwo;
    [SerializeField] private Sprite wallLvlTwoBroke;
    [SerializeField] private Sprite wallLvlThree;
    [SerializeField] private Sprite wallLvlThreeBroke;
    [SerializeField] private Sprite wallLvlFour;
    [SerializeField] private Sprite wallLvlFourBroke;

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer scaffoldSpriteRenderer;

    private Vector2 scaffoldPosition;
    private Vector2 startingPos;

    private int HP;
    private int actualLvlOfWall;

    private int hitPointsLvlOne = 25;
    private int hitPointsLvlTwo = 50;
    private int hitPointsLvlThree = 200;
    private int hitPointsLvlFour = 300;
    private int actualFullHp;
    private int halfOfFullHp;

    private bool playerPayButtonIsPressed;
    private bool playerHasCollidedWithWall;
    private int playerCoins;
    private int amountOfPaidCoins;
    private bool payingHasBeggun;

    private int requiredCoinsForUpgradeToOne = 1;
    private int requiredCoinsForUpgradeToTwo = 3;
    private int requiredCoinsForRepairLvlOneAndTwo = 1;
    private int requiredCoinsForUpgradeToThree = 6;
    private int requiredCoinsForRepairLvlThree = 3;
    private int requiredCoinsForUpgradeToFour = 9;
    private int requiredCoinsForRepairLvlFour = 4;
    private int requiredCoinsForUpgrade = 1;

    private bool wallIsUnderConstruction;
    private float buildTime;
    private bool wallHasBeenMarked;

    public event EventHandler<CallToWallArgs> OnCallBuilderToWall;
    public event EventHandler OnStopCallBuilderToWall;
    public event EventHandler<CallToWallArgs> WallHasBeenUpgraded;

    public class CallToWallArgs : EventArgs
    {
        public Vector2 positionOfWall;
    }


    void Start()
    {
        transform.tag = "EmptyWall";
        startingPos = transform.position;
        boxCollider2D = GetComponent<BoxCollider2D>();
        playerCharacter = GameObject.Find("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        scaffoldSpriteRenderer = scaffold.GetComponent<SpriteRenderer>();
        scaffoldSpriteRenderer.enabled = false;
        if (transform.position.x > 0) transform.localScale = new Vector2(-1, 1);
        AmountOfRequiredCoinsForUpgrade();
        townCenter = GameObject.Find("TownCenter");
        TownCenter tc = townCenter.GetComponent<TownCenter>();
        tc.OnTownCenterUpgrade += TcOnOnTownCenterUpgrade;
    }

    private void TcOnOnTownCenterUpgrade(object sender, TownCenter.TownCenterArgs e)
    {
        if (e.actualLvlOfCenter == 3 && actualLvlOfWall <= 0)
        {
            actualLvlOfWall = 1;
            ChangerToAnotherLvl();
        }
    }

    private void FixedUpdate()
    {
        // Debug.Log(HP);
        playerPayButtonIsPressed = playerCharacter.GetComponent<PickDropCoins>().payButtonIsPressed;

        if (playerHasCollidedWithWall && playerPayButtonIsPressed && !payingHasBeggun && !wallIsUnderConstruction)
        {
            if (actualLvlOfWall <= 3)
            {
                payingHasBeggun = true;
                AmountOfRequiredCoinsForUpgrade();
                InvokeRepeating("PayToWall", 0.2f, 0.5f);
            }
        }
        else if (!playerPayButtonIsPressed || !playerHasCollidedWithWall)
        {
            CancelInvoke("PayToWall");
            payingHasBeggun = false;
        }
        // wall is under construction
        if (buildTime >= 0 && wallIsUnderConstruction && wallHasBeenMarked)
            buildTime -= Time.fixedDeltaTime;
        // wall has been constructed
        else if (buildTime < 0f && wallIsUnderConstruction && wallHasBeenMarked)
        {
            WallHasBeenConstructed();
        }
    }

    private void WallHasBeenConstructed()
    {
        wallIsUnderConstruction = false;
        wallHasBeenMarked = false;
        transform.tag = "Wall";
        CancelInvoke("CallBuilderToWall");
        scaffoldSpriteRenderer.enabled = false;
        actualLvlOfWall++;
        ChangerToAnotherLvl();
        OnStopCallBuilderToWall?.Invoke(this, EventArgs.Empty);
        WallHasBeenUpgraded?.Invoke(this, new CallToWallArgs { positionOfWall = transform.position });
        foreach (var builder in assignedBuilders)
        {
            builder.GetComponent<BuilderBehaviour>().StopBuilding();
        }
        assignedBuilders.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("TrollCollider"))
        {
            HP -= 1;
            transform.tag = "WallUnderAttack";
            if (HP >= 1 && HP <= halfOfFullHp)
            {
                ChangeToBrokenWall();
            }
            else if (HP <= 0)
            {
                spriteRenderer.sprite = baseForWall;
                transform.tag = "EmptyWall";
                transform.position = startingPos;
            }
        }
        else if (collider2D.CompareTag("Player"))
        {
            playerHasCollidedWithWall = true;
        }
        else if (collider2D.CompareTag("Builder"))
        {
            if (wallHasBeenMarked)
            {
                wallIsUnderConstruction = true;
                assignedBuilders.Add(collider2D.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Builder"))
        {
            if (wallHasBeenMarked)
            {
                wallIsUnderConstruction = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            playerHasCollidedWithWall = false;
            amountOfPaidCoins = 0;
            CancelInvoke("PayToWall");
        }
        if (collider2D.CompareTag("Builder"))
            wallIsUnderConstruction = false;
    }

    private void PayToWall()
    {
        if (!wallHasBeenMarked && playerCharacter.GetComponent<PickDropCoins>().numberOfCoins >= 1)
        {
            playerCharacter.GetComponent<PickDropCoins>().numberOfCoins--;
            amountOfPaidCoins++;
            Debug.Log(playerCoins);
            Debug.Log(amountOfPaidCoins);
            if (amountOfPaidCoins >= requiredCoinsForUpgrade & !wallHasBeenMarked)
            {
                // paid
                buildTime = 10f;
                wallHasBeenMarked = true;
                transform.tag = "MarkedWall";
                ScaffoldPosition();
                scaffold.transform.position =  scaffoldPosition;
                scaffoldSpriteRenderer.enabled = true;
                amountOfPaidCoins = 0;
                CancelInvoke("PayToWall");
                InvokeRepeating("CallBuilderToWall", 0f, 5f);
            }
        }
        else
            CancelInvoke("PayToWall");
    }

    private void AmountOfRequiredCoinsForUpgrade()
    {
        if (actualLvlOfWall == 0) requiredCoinsForUpgrade = requiredCoinsForUpgradeToOne;
        else if (actualLvlOfWall == 1) requiredCoinsForUpgrade = requiredCoinsForUpgradeToTwo;
        else if (actualLvlOfWall == 2) requiredCoinsForUpgrade = requiredCoinsForUpgradeToThree;
        else if (actualLvlOfWall == 3) requiredCoinsForUpgrade = requiredCoinsForUpgradeToFour;
    }

    

    private void CallBuilderToWall()
    {
        OnCallBuilderToWall?.Invoke(this, new CallToWallArgs { positionOfWall = transform.position });
    }
    
    private void ChangerToAnotherLvl()
    {
        switch (actualLvlOfWall)
        {
            case 0:
                spriteRenderer.sprite = baseForWall;
                break;
            case 1:
                spriteRenderer.sprite = wallLvlOne;
                transform.position = new Vector2(transform.position.x, 0.64f);
                boxCollider2D.offset = new Vector2(boxCollider2D.offset.x , boxCollider2D.offset.y - 0.64f);
                HP = hitPointsLvlOne;
                halfOfFullHp = hitPointsLvlOne / 2;
                break;
            case 2:
                spriteRenderer.sprite = wallLvlTwo;
                transform.position = new Vector2(transform.position.x, 0.85f);
                boxCollider2D.offset = new Vector2(boxCollider2D.offset.x, boxCollider2D.offset.y - 0.21f);
                HP = hitPointsLvlTwo;
                halfOfFullHp = hitPointsLvlTwo / 2;
                break;
            case 3:
                spriteRenderer.sprite = wallLvlThree;
                transform.position = new Vector2(transform.position.x, 1.05f);
                boxCollider2D.offset = new Vector2(boxCollider2D.offset.x, boxCollider2D.offset.y - 0.2f);
                HP = hitPointsLvlThree;
                halfOfFullHp = hitPointsLvlThree / 2;
                break;
            case 4:
                spriteRenderer.sprite = wallLvlFour;
                transform.position = new Vector2(transform.position.x, 1.22f);
                boxCollider2D.offset = new Vector2(boxCollider2D.offset.x, boxCollider2D.offset.y - 0.17f);
                HP = hitPointsLvlFour;
                break;
        }
    }
    
    private void ScaffoldPosition()
    {
        Debug.Log(actualLvlOfWall);
        if (actualLvlOfWall == 0)
            scaffoldPosition = new Vector2(transform.position.x, 1.12f);
        else if (actualLvlOfWall == 1)
            scaffoldPosition = new Vector2(transform.position.x, 0.6f);
        else if (actualLvlOfWall == 2)
            scaffoldPosition = new Vector2(transform.position.x, 0.38f);
        else if (actualLvlOfWall == 3)
            scaffoldPosition = new Vector2(transform.position.x, 0.20f);
    }

    private void ChangeToBrokenWall()
    {
        if (actualLvlOfWall == 1)
        {
            spriteRenderer.sprite = wallLvlOneBroke;
        }        
        else if (actualLvlOfWall == 2)
        {
            spriteRenderer.sprite = wallLvlTwoBroke;
            actualFullHp = hitPointsLvlTwo;
        }            
        else if (actualLvlOfWall == 3)
        {
            spriteRenderer.sprite = wallLvlThreeBroke;
            actualFullHp = hitPointsLvlThree;
        }            
        else if (actualLvlOfWall == 4)
        {
            spriteRenderer.sprite = wallLvlFourBroke;
            actualFullHp = hitPointsLvlFour;
        }            
    }
}