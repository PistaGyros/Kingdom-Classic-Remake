using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Build.Reporting;
using System.Runtime.CompilerServices;

public class Wall : MonoBehaviour
{
    [SerializeField] private GameObject scaffold;
    private GameObject playerCharacter;

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

    private int HP;
    private int actualLvlOfWall;

    private int hitPointsLvlOne = 25;
    private int hitPointsLvlTwo = 50;
    private int hitPointsLvlThree = 200;
    private int hitPointsLvlFour = 300;
    private int actualFullHp;

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

    public class CallToWallArgs : EventArgs
    {
        public Vector2 positionOfWall;
    }


    void Start()
    {
        playerCharacter = GameObject.Find("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        scaffoldSpriteRenderer = scaffold.GetComponent<SpriteRenderer>();
        scaffoldSpriteRenderer.enabled = false;
        if (transform.position.x > 0) transform.localScale = new Vector2(-1, 1);
        AmountOfRequiredCoinsForUpgrade();
    }

    private void FixedUpdate()
    {
        Debug.Log(buildTime);
        playerCoins = playerCharacter.GetComponent<PickDropCoins>().numberOfCoins;
        playerPayButtonIsPressed = playerCharacter.GetComponent<PickDropCoins>().payButtonIsPressed;

        if (playerHasCollidedWithWall && playerPayButtonIsPressed && !payingHasBeggun && !wallIsUnderConstruction)
        {
            if (actualLvlOfWall <= 3)
            {
                payingHasBeggun = true;
                AmountOfRequiredCoinsForUpgrade();
                StartInvoke();
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
            wallIsUnderConstruction = false;
            wallHasBeenMarked = false;
            transform.tag = "Wall";
            CancelInvoke("CallBuilderToWall");
            scaffoldSpriteRenderer.enabled = false;
            actualLvlOfWall++;
            ChangerToAnotherLvl();
            OnStopCallBuilderToWall?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("TrollCollider"))
        {
            HP -= 1;
            transform.tag = "WallUnderAttack";
            if (HP <= (actualFullHp / 2))
            {
                ChangeToBrokenWall();
            }
            else if (HP <= 0)
                spriteRenderer.sprite = baseForWall;
        }
        if (collider2D.CompareTag("Player"))
        {
            playerHasCollidedWithWall = true;
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

    private void StartInvoke()
    {
        InvokeRepeating("PayToWall", 0.2f, 0.5f);
    }

    private void PayToWall()
    {
        if (playerCharacter.GetComponent<PickDropCoins>().numberOfCoins >= 1 && !wallHasBeenMarked)
        {
            playerCharacter.GetComponent<PickDropCoins>().numberOfCoins--;
            amountOfPaidCoins++;
            Debug.Log(playerCoins);
            Debug.Log(amountOfPaidCoins);
            if (amountOfPaidCoins >= requiredCoinsForUpgrade & !wallHasBeenMarked)
            {
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
        {
            CancelInvoke("PayToWall");
        }

    }

    private void AmountOfRequiredCoinsForUpgrade()
    {
        if (actualLvlOfWall == 0) requiredCoinsForUpgrade = requiredCoinsForUpgradeToOne;
        else if (actualLvlOfWall == 1) requiredCoinsForUpgrade = requiredCoinsForUpgradeToTwo;
        else if (actualLvlOfWall == 2) requiredCoinsForUpgrade = requiredCoinsForUpgradeToThree;
        else if (actualLvlOfWall == 3) requiredCoinsForUpgrade = requiredCoinsForUpgradeToFour;
    }

    private void ChangerToAnotherLvl()
    {
        if (actualLvlOfWall == 0)
            spriteRenderer.sprite = baseForWall;
        else if (actualLvlOfWall == 1)
        {
            spriteRenderer.sprite = wallLvlOne;
            transform.position = new Vector2(transform.position.x, 0.64f);
            HP = hitPointsLvlOne;
        }            
        else if (actualLvlOfWall == 2)
        {
            spriteRenderer.sprite = wallLvlTwo;
            transform.position = new Vector2(transform.position.x, 0.85f);
            HP = hitPointsLvlTwo;
        }
        else if (actualLvlOfWall == 3)
        {
            spriteRenderer.sprite = wallLvlThree;
            transform.position = new Vector2(transform.position.x, 1.05f);
            HP = hitPointsLvlTwo;
        }
        else if (actualLvlOfWall == 4)
        {
            spriteRenderer.sprite = wallLvlFour;
            transform.position = new Vector2(transform.position.x, 1.22f);
            HP = hitPointsLvlFour;
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

    private void CallBuilderToWall()
    {
        OnCallBuilderToWall?.Invoke(this, new CallToWallArgs { positionOfWall = transform.position });
    }

    private void ChangeToBrokenWall()
    {
        if (actualLvlOfWall == 1)
        {
            spriteRenderer.sprite = wallLvlOneBroke;
            actualFullHp = hitPointsLvlOne;
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