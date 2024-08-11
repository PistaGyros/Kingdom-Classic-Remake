using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class Farm : MonoBehaviour
{
    [SerializeField] private GameObject farmLand;
    
    private GameObject player;
    [SerializeField] private BoxCollider2D farmBoxCollider;
    private SpriteRenderer farmSpriteRenderer;
    [SerializeField] private GameObject farmOutline;
    private SpriteRenderer farmOutlineSpriteRenderer;
    private GameObject townCenter;

    private int farmLevel;
    private int numberOfFarmLands;
    private float startPos;

    private int[] posesOfFarmLands = new int[6] { -4, 4, -8, 8, -12, 12};
    private List<GameObject> farmLands = new List<GameObject>();
    private int[] requiredCoinsForUp = new int[2] { 6, 8};
    [SerializeField] private List<Sprite> farmSprites;
    [SerializeField] private List<Sprite> farmScaffoldSprites;
    
    private bool isFarmActive;
    private bool playerHasCollided;
    private bool payingHasBegun;
    private bool farmIsUnderConstruct;
    private bool farmHasBeenMarked;
    private int amountOfPaidCoins;
    private float buildTime;
    private bool builderHasCollided;

    public event EventHandler<CallToFarmArgs> OnCallBuildersToFarm;
    public event EventHandler OnStopCallBuildersToFarm;

    public class CallToFarmArgs : EventArgs
    {
        public float posOfFarm;
    }

    private void Start()
    {
        player = GameObject.Find("Player");
        townCenter = GameObject.Find("TownCenter");
        TownCenter townCenterScript = townCenter.GetComponent<TownCenter>();
        townCenterScript.OnTownCenterUpgrade += TownCenterScriptOnOnTownCenterUpgrade;
        farmSpriteRenderer = GetComponent<SpriteRenderer>();
        farmOutlineSpriteRenderer = farmOutline.GetComponent<SpriteRenderer>();
        farmBoxCollider.enabled = false;
        startPos = transform.position.x;
    }

    private void TownCenterScriptOnOnTownCenterUpgrade(object sender, TownCenter.TownCenterArgs e)
    {
        if (e.actualLvlOfCenter == 3)
        {
            Debug.Log("Farms has been activated");
            ActivateFarm();
        }
    }

    private void FixedUpdate()
    {
        if (isFarmActive)
        {
            if (playerHasCollided && farmLevel <= 1)
            {
                bool playerPayButtonIsPressed = player.GetComponent<PickDropCoins>().payButtonIsPressed;
                int playersAmountOfCoins = player.GetComponent<PickDropCoins>().numberOfCoins;
                if (playerPayButtonIsPressed && playersAmountOfCoins >= 1 && !payingHasBegun && !farmIsUnderConstruct)
                {
                    payingHasBegun = true;
                    Debug.Log("Paying has begun");
                    InvokeRepeating("PayingToFarm", 0.2f, 0.5f);
                }
                else if (!playerHasCollided || !playerPayButtonIsPressed || playersAmountOfCoins < 1)
                {
                    CancelInvoke("PayingToFarm");
                }
            }
            if (farmHasBeenMarked && builderHasCollided && buildTime > 0)
            {
                buildTime -= Time.fixedDeltaTime;
                if (buildTime <= 0)
                {
                    // farm has been upgraded
                    UpgradeFarm();
                }
            }
        }
    }

    private void PayingToFarm()
    {
        int playersAmountOfCoins = player.GetComponent<PickDropCoins>().numberOfCoins;
        if (!farmHasBeenMarked && playersAmountOfCoins >= 1)
        {
            player.GetComponent<PickDropCoins>().numberOfCoins--;
            amountOfPaidCoins++;
            Debug.Log(playersAmountOfCoins);
            Debug.Log(amountOfPaidCoins);
            if (amountOfPaidCoins == requiredCoinsForUp[farmLevel])
            {
                // farm level up, farm has been marked
                Debug.Log("Successful payment");
                MarkFarm();
            }
        }
    }

    private void MarkFarm()
    {
        farmHasBeenMarked = true;
        buildTime = 15;
        transform.tag = "MarkedFarm";
        amountOfPaidCoins = 0;
        // scaffold sprite
        farmSpriteRenderer.sprite = farmScaffoldSprites[farmLevel];
        CancelInvoke("PayingToFarm");
        InvokeRepeating("CallBuilderToFarm", 0f, 5f);
    }

    public void ActivateFarm()
    {
        isFarmActive = true;
        farmBoxCollider.enabled = true;
        transform.tag = "OpenFarm";
    }

    private void UpgradeFarm()
    {
        farmLevel++;
        farmSpriteRenderer.sprite = farmSprites[farmLevel];
        if (farmLevel <= 1)
        {
            transform.tag = "OpenFarm";
        }
        else
        {
            transform.tag = "Farm";
        }

        farmHasBeenMarked = false;
        farmIsUnderConstruct = false;
        CancelInvoke("CallBuilderToFarm");
        OnStopCallBuildersToFarm?.Invoke(this, EventArgs.Empty);
    }

    private void CallBuilderToFarm()
    {
        OnCallBuildersToFarm?.Invoke(this, new CallToFarmArgs{posOfFarm = startPos});
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Farmer") && numberOfFarmLands <= 6)
        {
            PlaceNewFarmLand();
            collider2D.GetComponent<Farmer>().GoToYourNewFarmLand(farmLands[-1]);
            numberOfFarmLands++;
            if (numberOfFarmLands >= 6)
                transform.tag = "Farm";
        }
        else if (collider2D.CompareTag("Player"))
        {
            // activate outline
            playerHasCollided = true;
        }
        else if (collider2D.CompareTag("Builder"))
        {
            builderHasCollided = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Builder"))
            builderHasCollided = true;
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            playerHasCollided = false;
        }
        else if (collider2D.CompareTag("Builder"))
            builderHasCollided = false;
    }

    private void PlaceNewFarmLand()
    {
        farmLands.Add(Instantiate(farmLand, new Vector2(transform.position.x
                                                        + posesOfFarmLands[numberOfFarmLands], 0.35f), Quaternion.identity));
    }
}
