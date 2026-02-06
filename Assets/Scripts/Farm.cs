using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class Farm : MonoBehaviour
{
    [SerializeField] private GameObject farmLand;
    
    private GameObject player;
    private PickDropCoins pickDropCoinsPlayer;
    [SerializeField] private BoxCollider2D farmBoxCollider;
    [SerializeField] private GameObject farmOutline;
    private SpriteRenderer farmOutlineSpriteRenderer;
    [SerializeField] private GameObject forGround;
    private SpriteRenderer forGroundSpriteRenderer;
    private GameObject townCenter;

    private int farmLevel;
    private int numberOfFarmLands;
    private float startPos;

    private int[] posesOfFarmLands = new int[6] { -4, 4, -8, 8, -12, 12};
    private List<GameObject> farmLands = new List<GameObject>();
    private int[] requiredCoinsForUp = new int[2] { 6, 8};
    [SerializeField] private List<Sprite> farmSprites;
    [SerializeField] private List<Sprite> farmScaffoldSprites;
    [SerializeField] private List<Sprite> farmOutlineSprites;

    private bool isFarmActive;
    private bool playerHasCollided;
    private bool payingHasBegun;
    private bool farmIsUnderConstruct;
    private bool farmHasBeenMarked;
    private int amountOfPaidCoins;
    
    private float buildTime;
    private bool builderHasCollided;
    private List<GameObject> activeBuilders = new List<GameObject>();

    public event EventHandler<CallToFarmArgs> OnCallBuildersToFarm;
    public event EventHandler OnStopCallBuildersToFarm;

    public class CallToFarmArgs : EventArgs
    {
        public float posOfFarm;
    }

    private void Start()
    {
        player = GameObject.Find("Player");
        pickDropCoinsPlayer = player.GetComponent<PickDropCoins>();
        townCenter = GameObject.Find("TownCenter");
        TownCenter townCenterScript = townCenter.GetComponent<TownCenter>();
        townCenterScript.OnTownCenterUpgrade += TownCenterScriptOnOnTownCenterUpgrade;
        farmOutlineSpriteRenderer = farmOutline.GetComponent<SpriteRenderer>();
        forGroundSpriteRenderer = forGround.GetComponent<SpriteRenderer>();
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
    
    public void ActivateFarm()
    {
        isFarmActive = true;
        farmBoxCollider.enabled = true;
        transform.tag = "OpenFarm";
    }

    private void FixedUpdate()
    {
        if (isFarmActive)
        {
            if (playerHasCollided && farmLevel <= 1)
            {
                bool playerPayButtonIsPressed = pickDropCoinsPlayer.payButtonIsPressed;
                int playersAmountOfCoins = pickDropCoinsPlayer.numberOfCoins;
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
            if (farmHasBeenMarked & builderHasCollided & buildTime > 0)
            {
                buildTime -= Time.fixedDeltaTime * activeBuilders.Count;
                Debug.Log("Time to complete building remaining is: " + buildTime);
                if (buildTime <= 0)
                {
                    // farm has been upgraded
                    Debug.Log("Building farm has been finished");
                    UpgradeFarm();
                }
            }
        }
    }

    private void PayingToFarm()
    {
        int playersAmountOfCoins = pickDropCoinsPlayer.numberOfCoins;
        if (!farmHasBeenMarked && playersAmountOfCoins >= 1)
        {
            pickDropCoinsPlayer.numberOfCoins--;
            amountOfPaidCoins++;
            Debug.Log(playersAmountOfCoins);
            Debug.Log(amountOfPaidCoins);
            if (amountOfPaidCoins == requiredCoinsForUp[farmLevel])
            {
                // farm has been marked
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
        forGroundSpriteRenderer.sprite = farmScaffoldSprites[farmLevel];
        CancelInvoke("PayingToFarm");
        InvokeRepeating("CallBuilderToFarm", 0f, 5f);
    }
    

    private void UpgradeFarm()
    {
        farmLevel++;
        forGroundSpriteRenderer.sprite = farmSprites[farmLevel];
        farmOutlineSpriteRenderer.sprite = farmOutlineSprites[farmLevel];
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
        StopWorkersWork();
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
            farmOutlineSpriteRenderer.enabled = true;
            playerHasCollided = true;
        }
        else if (collider2D.CompareTag("Builder"))
        {
            builderHasCollided = true;
            if (activeBuilders.Count < 2)
            {
                activeBuilders.Add(collider2D.gameObject);
            }
            else
            {
                CancelInvoke("CallBuilderToFarm");
            }
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
            farmOutlineSpriteRenderer.enabled = false;
            playerHasCollided = false;
        }
        else if (collider2D.CompareTag("Builder"))
        {
            activeBuilders.Remove(collider2D.gameObject);
            if (activeBuilders.Count <= 1)
            {
                InvokeRepeating("CallBuilderToFarm", 0f, 5f);
            }

            if (activeBuilders.Count <= 0)
                builderHasCollided = false;
        }
    }

    private void StopWorkersWork()
    {
        foreach (var worker in activeBuilders)
        {
            worker.GetComponent<BuilderBehaviour>().StopBuilding();
        }
    }

    private void PlaceNewFarmLand()
    {
        farmLands.Add(Instantiate(farmLand, new Vector2(transform.position.x
                                                        + posesOfFarmLands[numberOfFarmLands], 0.35f), Quaternion.identity));
    }
}
