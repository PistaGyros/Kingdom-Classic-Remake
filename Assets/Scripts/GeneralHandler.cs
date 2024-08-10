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
    [SerializeField] private GameObject scytheMarket;
    [SerializeField] private GameObject globalLight;
    private GameObject[] archers;
    private GameObject[] freeFarmers;

    private List<GameObject> archersOnEast = new List<GameObject>();
    private List<GameObject> archersOnWest = new List<GameObject>();

    private List<GameObject> openFarms = new List<GameObject>();

    private int actualDay;


    void Start()
    {
        FindFreeArchers();
        bowMarket = GameObject.Find("BowMarket");
        BowMarketZero market = bowMarket.GetComponent<BowMarketZero>();
        market.OnBowPickedUp += MarketOnOnBowPickedUp;
        DayAndNightCycleBehaviour dayAndNightCycleBehaviour = globalLight.GetComponent<DayAndNightCycleBehaviour>();
        dayAndNightCycleBehaviour.OnChangeToNextDay += DayAndNightCycleBehaviourOnOnChangeToNextDay;
        ScytheMarket hoeMarket = scytheMarket.GetComponent<ScytheMarket>();
        hoeMarket.OnNewFarmer += HoeMarketOnOnNewFarmer;

    }

    private void HoeMarketOnOnNewFarmer(object sender, EventArgs e)
    {
        FreeFarmerSorter();
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
        Debug.Log("OK");
        foreach (GameObject archer in archers)
        {
            Debug.Log("OK");
            if (archersOnEast.Count <= archersOnWest.Count)
            {
                Debug.Log("OK");
                archersOnEast.Add(archer);
                archer.GetComponentInChildren<ArcherBehaviour>().Invoke("NewEastBorder", 0.5f);
            }
            else
            {
                Debug.Log("OK");
                archersOnWest.Add(archer);
                archer.GetComponentInChildren<ArcherBehaviour>().Invoke("NewWestBorder", 0.5f);
            }
        }
    }

    private void FreeFarmerSorter()
    {
        freeFarmers = GameObject.FindGameObjectsWithTag("FreeFarmer");
        foreach (var freeFarmer in freeFarmers)
        {
            GameObject[] openFarms = GameObject.FindGameObjectsWithTag("OpenFarm");
            foreach (var openFarm in openFarms)
            {
                this.openFarms.Add(openFarm);
            }
            this.openFarms.OrderBy(wall => wall.transform.position.x);
            GameObject nearestOpenFarm = openFarms[0];
            freeFarmer.GetComponent<Farmer>().GoToYourFarm(nearestOpenFarm.transform.position.x);
        }
    }
}
