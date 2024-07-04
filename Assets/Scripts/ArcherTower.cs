using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private GameObject tower;
    private SpriteRenderer towerSpriteRenderer;
    [SerializeField] private GameObject treeChecker;

    [SerializeField] private Sprite[] towersSprites;
    [SerializeField] private Sprite[] scaffoldSprites;

    private Vector2 startingPos;
    private Vector2[] posOfTower = new Vector2[5]
    {
        new Vector2(0, 0),
        new Vector2(0, 0.45f),
        new Vector2(0, 0.85f),
        new Vector2(0, 1),
        new Vector2(0, 1.7f)
    };

    private Vector2[] posOfScaffold = new Vector2[5]
    {
        new Vector2(0, 0.85f),
        new Vector2(0, 1.4f),
        new Vector2(0, 1.1f),
        new Vector2(0, 1.7f),
        new Vector2(0, 0)
    };

    private int actualLvlOfTower;
    private int[] costOfNextUp = new int[5] { 0, 3, 5, 7, 9};
    public int numberOfArchers;
    private int[] numberOfAllowedArchers = new int[5] { 0, 1, 1, 2, 3 };
    public bool playerHasCollided;
    private bool towerCanBeUpgraded;
    private bool playerPayButtonIsPressed;
    private bool payingHasBegun;
    private int amountOfPaidCoins;
    private float buildTimeLeft;
    private bool towerIsUnderConstruction;
    private static string upgradableTowerTag = "UpgradableTower";
    private static string towerUnderConstructTag = "Tower";

    public List<GameObject> activeWorkers;

    public event EventHandler<ArcherTowerArgs> OnCallBuildersToTower;
    public event EventHandler OnStopCallBuildersToTower;

    public class ArcherTowerArgs : EventArgs
    {
        public Vector2 actualPos;
    }
    
    void Start()
    {
        startingPos = transform.position;
        player = GameObject.Find("Player");
        towerSpriteRenderer = tower.GetComponent<SpriteRenderer>();
        towerCanBeUpgraded = true;
        switch (transform.position.x)
        {
            case < 0:
                transform.localScale = new Vector2(-1, 1);
                break;
        }
    }

    
    void FixedUpdate()
    {
        playerPayButtonIsPressed = player.GetComponent<PickDropCoins>().payButtonIsPressed;

        if (playerHasCollided && playerPayButtonIsPressed)
        {
            if (towerCanBeUpgraded && !payingHasBegun)
            {
                payingHasBegun = true;
                InvokeRepeating("PayToTower", 0.2f, 0.5f);
            }
        }
        else if (!playerPayButtonIsPressed || !playerHasCollided)
        {
            payingHasBegun = false;
            amountOfPaidCoins = 0;
            CancelInvoke("PayToTower");
        }

        if (towerIsUnderConstruction && activeWorkers.Count >= 1)
        {
            buildTimeLeft -= Time.fixedDeltaTime * activeWorkers.Count;
            if (buildTimeLeft <= 0)
            {
                // tower has been constructed
                UpgradeTower();
                towerIsUnderConstruction = false;
                CancelInvoke("CallBuildersToTower");
                LetWorkersFree();
            }
        }
    }

    private void PayToTower()
    {
        int playerCoins = player.GetComponent<PickDropCoins>().numberOfCoins;
        if (towerCanBeUpgraded && playerCoins >= 1)
        {
            player.GetComponent<PickDropCoins>().numberOfCoins--;
            amountOfPaidCoins++;
            Debug.Log("Player has paid, actual paid coins: " + amountOfPaidCoins);
            if (amountOfPaidCoins == costOfNextUp[actualLvlOfTower + 1])
            {
                // successful payment
                Debug.Log("Payment has been successful");
                tower.transform.tag = towerUnderConstructTag;
                amountOfPaidCoins = 0;
                towerCanBeUpgraded = false;
                ScaffoldUp();
                InvokeRepeating("CallBuildersToTower", 0f, 5f);
                CancelInvoke("PayToTower");
                // building has began
                towerIsUnderConstruction = true;
                buildTimeLeft = 20f;
            }
        }
        else
            CancelInvoke("PayToTower");
    }

    private void Delay()
    {
        towerCanBeUpgraded = true;
        tower.transform.tag = upgradableTowerTag;
    }

    private void UpgradeTower()
    {
        Debug.Log("Tower has been constructed");
        actualLvlOfTower++;
        towerSpriteRenderer.sprite = towersSprites[actualLvlOfTower];
        tower.transform.position = new Vector2(transform.position.x, posOfTower[actualLvlOfTower].y);
        Invoke("Delay", 2f);
    }

    private void ScaffoldUp()
    {
        towerSpriteRenderer.sprite = scaffoldSprites[actualLvlOfTower];
        tower.transform.position = new Vector2(transform.position.x, posOfScaffold[actualLvlOfTower].y);
    }

    private void CallBuildersToTower()
    {
        OnCallBuildersToTower?.Invoke(this,
            new ArcherTowerArgs {actualPos = new Vector2(transform.position.x, transform.position.y)} );
    }

    public void BuilderHasCollided()
    {
        if (towerIsUnderConstruction)
        {
            switch (activeWorkers.Count)
            {
                case 2:
                    CancelInvoke("CallBuildersToTower");
                    OnStopCallBuildersToTower?.Invoke(this, EventArgs.Empty);
                    break;
            }
            LetWorkerWork();
        }
    }

    public void BuilderHasLeft()
    {
        if (towerIsUnderConstruction)
        {
            switch (activeWorkers.Count)
            {
                case 0 | 1:
                    InvokeRepeating("CallBuildersToTower", 0f, 5f);
                    break;
            }
        }
    }

    private void LetWorkersFree()
    {
        foreach (GameObject worker in activeWorkers)
        {
            Debug.Log(worker);
            worker.GetComponent<BuilderBehaviour>().StopBuilding();
        }
    }

    private void LetWorkerWork()
    {
        foreach (GameObject worker in activeWorkers)
        {
            Debug.Log(worker);
            worker.GetComponent<BuilderBehaviour>().StartWorking();
        }
    }
}
