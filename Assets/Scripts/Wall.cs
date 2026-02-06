using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class Wall : MonoBehaviour
{
    private GameObject playerCharacter;
    private List<GameObject> assignedBuilders = new List<GameObject>();
    private BoxCollider2D boxCollider2D;
    private GameObject townCenter;

    [SerializeField] private List<Sprite> wallsSprites;
    [SerializeField] private List<Sprite> scaffoldWallSprites;
    [SerializeField] private List<Sprite> wallsBrokenSprites;

    private SpriteRenderer spriteRenderer;
    
    private Vector2 startingPos;
    private float scaffoldsPos = 1.115f;

    private float[] posesWalls = new float[5] {0, 0.64f, 0.85f, 1.05f, 1.22f};

    private int HP;
    private int actualLvlOfWall;
    
    private int actualFullHp;
    private int halfOfFullHp;

    private int[] fullHps = new int[5] { 0, 25, 50, 200, 300 };

    private bool playerPayButtonIsPressed;
    private bool playerHasCollidedWithWall;
    private int playerCoins;
    private int amountOfPaidCoins;
    private bool payingHasBeggun;

    private int[] requiredCoinsForUpgrades = new int[4] { 1, 3, 6, 9 };
    private int[] requiredCoinsForRepairs = new int[5] { 0, 1, 1, 2, 4 };

    private bool wallIsUnderConstruction;
    private float buildTime;
    private bool wallHasBeenMarked;

    public event EventHandler<CallToWallArgs> OnCallBuilderToWall;
    public event EventHandler OnStopCallBuilderToWall;
    public event EventHandler<CallToWallArgs> OnWallHasBeenUpgraded;

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
        if (transform.position.x > 0) transform.localScale = new Vector2(-1, 1);
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
        playerPayButtonIsPressed = playerCharacter.GetComponent<PickDropCoins>().payButtonIsPressed;

        if (playerHasCollidedWithWall && playerPayButtonIsPressed && !payingHasBeggun && !wallIsUnderConstruction)
        {
            if (actualLvlOfWall <= 3)
            {
                payingHasBeggun = true;
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
        actualLvlOfWall++;
        ChangerToAnotherLvl();
        OnStopCallBuilderToWall?.Invoke(this, EventArgs.Empty);
        OnWallHasBeenUpgraded?.Invoke(this, new CallToWallArgs { positionOfWall = transform.position });
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
                spriteRenderer.sprite = wallsSprites[0];
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
            if (amountOfPaidCoins >= requiredCoinsForUpgrades[actualLvlOfWall] & !wallHasBeenMarked)
            {
                // payment was succesful
                buildTime = 10f;
                wallHasBeenMarked = true;
                transform.tag = "MarkedWall";
                spriteRenderer.sprite = scaffoldWallSprites[actualLvlOfWall];
                transform.position = new Vector2(startingPos.x, scaffoldsPos);
                amountOfPaidCoins = 0;
                CancelInvoke("PayToWall");
                InvokeRepeating("CallBuilderToWall", 0f, 5f);
            }
        }
        else
            CancelInvoke("PayToWall");
    }


    private void CallBuilderToWall()
    {
        OnCallBuilderToWall?.Invoke(this, new CallToWallArgs { positionOfWall = transform.position });
    }
    
    private void ChangerToAnotherLvl()
    {
        spriteRenderer.sprite = wallsSprites[actualLvlOfWall];
        transform.position = new Vector2(transform.position.x, posesWalls[actualLvlOfWall]);
        halfOfFullHp = fullHps[actualLvlOfWall] / 2;
    }
    
    
    private void ChangeToBrokenWall()
    {
        switch (actualLvlOfWall)
        {
            case 1:
                spriteRenderer.sprite = wallsBrokenSprites[actualLvlOfWall];
                break;
        }
    }
}