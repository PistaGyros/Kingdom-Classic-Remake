using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehaviour : MonoBehaviour
{
    [SerializeField] GameObject coin;
    private GameObject generalHandler;
    int numberOfCoins = 5;
    bool canPickUp;
    private GameObject globalLight;
    private GameObject homeBorder;



    void Start()
    {
        canPickUp = true;

        globalLight = GameObject.Find("GlobalLight2D");
        // init of events
        GeneralHandler generalHandler = globalLight.GetComponent<GeneralHandler>();
        generalHandler.GoToEast += GeneralHandlerOnGoToEast;
        generalHandler.GoToWest += GeneralHandlerOnGoToWest;
        DayAndNightCycleBehaviour dayAndNightCycleBehaviour = globalLight.GetComponent<DayAndNightCycleBehaviour>();
        dayAndNightCycleBehaviour.OnChangeToNextDay += DayAndNightCycleBehaviourOnOnChangeToNextDay;
        dayAndNightCycleBehaviour.OnChangeToSunSet += ChangeToSunSetOnOnChangeToSunSet;
    }

    private void GeneralHandlerOnGoToWest(object sender, EventArgs e)
    {

        if (transform.parent.gameObject.tag == "FreeArcher")
        {
            homeBorder = GameObject.Find("WestBordersOfKingdom");
            transform.parent.gameObject.tag = "Archer";
            Debug.Log("It works");
        }
    }

    private void GeneralHandlerOnGoToEast(object sender, EventArgs e)
    {
        if (transform.parent.gameObject.tag == "FreeArcher")
        {
            homeBorder = GameObject.Find("EastBordersOfKingdom");
            transform.parent.gameObject.tag = "Archer";
            Debug.Log("It works");
        }
    }

    private void DayAndNightCycleBehaviourOnOnChangeToNextDay(object sender, EventArgs e)
    {
        StartWander();
    }

    private void ChangeToSunSetOnOnChangeToSunSet(object sender, EventArgs e)
    {
        ReturnToBorders();
    }


    void FixedUpdate()
    {

    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            canPickUp = false;
            Invoke("TurnCanPickUpBackToTrue", 2f);
        }
        else if (collider2D.CompareTag("PlayerCoins") || collider2D.CompareTag("Coins"))
        {
            PickUpCoin();
        }
        
    }

    private void OnTriggerStay2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            DropCoin();
            Invoke("DropCoin", 0.5f);
        }

    }

    private void PickUpCoin()
    {
        if (canPickUp)
        {
            numberOfCoins++;
        }        
    }

    private void TurnCanPickUpBackToTrue()
    {
        canPickUp = true;
    }

    private void DropCoin()
    {
        if(numberOfCoins > 0)
        {
            numberOfCoins--;
            Instantiate(coin, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        }        
    }

    private void StartWander()
    {
        
    }

    private void ReturnToBorders()
    {
        
    }
}