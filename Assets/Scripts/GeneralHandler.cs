using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Timers;
using System.Threading.Tasks;

public class GeneralHandler : MonoBehaviour
{
    [SerializeField] private GameObject bowMarket;
    [SerializeField] private GameObject globalLight;
    private GameObject archer = null;
    [SerializeField] private GameObject taxChest;

    private List<GameObject> archersOnEast = new List<GameObject>();
    private List<GameObject> archersOnWest = new List<GameObject>();

    private int actualDay;

    public event EventHandler GoToEast;
    public event EventHandler GoToWest;
    
    
    void Start()
    {
        bowMarket = GameObject.Find("BowMarket");
        BowMarketZero market = bowMarket.GetComponent<BowMarketZero>();
        market.OnBowPickedUp += MarketOnOnBowPickedUp;
        DayAndNightCycleBehaviour dayAndNightCycleBehaviour = globalLight.GetComponent<DayAndNightCycleBehaviour>();
        dayAndNightCycleBehaviour.OnChangeToNextDay += DayAndNightCycleBehaviourOnOnChangeToNextDay;
    }

    private void DayAndNightCycleBehaviourOnOnChangeToNextDay(object sender, DayAndNightCycleBehaviour.ChangeToNextDayArgs e)
    {
        SpawnTaxChest();
        Debug.Log("Tax chest has spawned");
    }

    private void MarketOnOnBowPickedUp(object sender, EventArgs e)
    {
        archer = GameObject.FindWithTag("FreeArcher");
        if (archersOnEast.Count <= archersOnWest.Count)
        {
            Invoke("GoToEastBorders", 1f);
            archersOnEast.Add(archer);
        }
        else
        {
            Invoke("GoToWestBorders", 1f);
            archersOnWest.Add(archer);
        }
    }


    void FixedUpdate()
    {
        
    }

    private void GoToEastBorders()
    {
        GoToEast?.Invoke(this, EventArgs.Empty);
    }

    private void GoToWestBorders()
    {
        GoToWest?.Invoke(this, EventArgs.Empty);
    }

    private void SpawnTaxChest()
    {
        Instantiate(taxChest, new Vector2(3.5f, 0.17f), Quaternion.identity);
    }
}
