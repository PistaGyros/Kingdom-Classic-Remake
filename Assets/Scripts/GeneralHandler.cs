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
    private GameObject[] archers = null;

    private List<GameObject> archersOnEast = new List<GameObject>();
    private List<GameObject> archersOnWest = new List<GameObject>();

    private int actualDay;


    void Start()
    {
        FindFreeArchers();
        bowMarket = GameObject.Find("BowMarket");
        BowMarketZero market = bowMarket.GetComponent<BowMarketZero>();
        market.OnBowPickedUp += MarketOnOnBowPickedUp;
        DayAndNightCycleBehaviour dayAndNightCycleBehaviour = globalLight.GetComponent<DayAndNightCycleBehaviour>();
        dayAndNightCycleBehaviour.OnChangeToNextDay += DayAndNightCycleBehaviourOnOnChangeToNextDay;
    }

    private void DayAndNightCycleBehaviourOnOnChangeToNextDay(object sender, DayAndNightCycleBehaviour.ChangeToNextDayArgs e)
    {
        Debug.Log("Tax chest has spawned");
    }

    private void MarketOnOnBowPickedUp(object sender, EventArgs e)
    {
        FindFreeArchers();
    }


    void FixedUpdate()
    {
        
    }
    

    private void FindFreeArchers()
    {
        archers = GameObject.FindGameObjectsWithTag("FreeArcher");
        foreach (GameObject archer in archers)
        {
            if (archersOnEast.Count <= archersOnWest.Count)
            {
                archersOnEast.Add(archer);
                archer.GetComponent<ArcherBehaviour>().Invoke("NewEastBorder", 0f);
            }
            else
            {
                archersOnWest.Add(archer);
                archer.GetComponent<ArcherBehaviour>().Invoke("NewWestBorder", 0f);
            }
        }
    }
}
