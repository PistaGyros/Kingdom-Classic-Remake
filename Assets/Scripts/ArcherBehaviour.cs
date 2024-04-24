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
    private Rigidbody2D archerRigidbody2D;
    private Animator archerAnimator;
    private int archerSpeed = 5;
    private int archerDirection;
    private string isWalking = "IsWalking";



    void Start()
    {
        archerRigidbody2D = gameObject.GetComponentInParent<Rigidbody2D>();
        archerAnimator = gameObject.GetComponentInParent<Animator>();
        canPickUp = true;
        globalLight = GameObject.Find("GlobalLight2D");
        
        // init of events
        GeneralHandler generalHandler = globalLight.GetComponent<GeneralHandler>();
        DayAndNightCycleBehaviour dayAndNightCycleBehaviour = globalLight.GetComponent<DayAndNightCycleBehaviour>();
        dayAndNightCycleBehaviour.OnChangeToNextDay += DayAndNightCycleBehaviourOnOnChangeToNextDay;
        dayAndNightCycleBehaviour.OnChangeToSunSet += ChangeToSunSetOnOnChangeToSunSet;
    }
    
    private void BorderOfKingdomOnOnPosOfBorderHasChanged(object sender, EventArgs e)
    {
        
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
        if (homeBorder != null)
            Run();
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            canPickUp = false;
            InvokeRepeating("DropCoin", 0f, 0.3f);
            Invoke("PickUpAgain", 5f);
        }
        else if (collider2D.CompareTag("PlayerCoins") || collider2D.CompareTag("Coins"))
        {
            PickUpCoin();
        }
        else if (collider2D.CompareTag("PositionForArchers"))
        {
            archerDirection = 0;
            archerAnimator.SetBool("IsWalking", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            
        }
    }

    private void PickUpAgain()
    {
        canPickUp = true;
    }

    private void PickUpCoin()
    {
        if (canPickUp)
        {
            numberOfCoins++;
            archerAnimator.Play("PickUpCoin");
        }        
    }

    private void DropCoin()
    {
        if (numberOfCoins >= 1)
        {
            numberOfCoins--;
            Instantiate(coin, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        }
        else
        {
            CancelInvoke("DropCoin");
        }
    }

    private void StartWander()
    {
        
    }

    private void ReturnToBorders()
    {
        BorderOfKingdom borderOfKingdom = homeBorder.GetComponent<BorderOfKingdom>();
        borderOfKingdom.OnPosOfBorderHasChanged += BorderOfKingdomOnOnPosOfBorderHasChanged;
        if (transform.position.x < homeBorder.transform.position.x)
            archerDirection = 1;
        else
            archerDirection = -1;
        gameObject.transform.parent.localScale = new Vector2(archerDirection, 1);
        archerAnimator.SetBool("IsWalking", true);
    }

    private void Run()
    {
        Vector2 archerVelocity = new Vector2(archerDirection, 0);
        archerRigidbody2D.velocity = archerVelocity * archerSpeed;
    }

    private void WaitForBorderToExist()
    {
        if (homeBorder.transform.position.x > -1000)
        {
            ReturnToBorders();
            CancelInvoke("WaitForBorderToExist");
        }
    }

    private void NewEastBorder()
    {
        if (transform.parent.CompareTag("FreeArcher"))
        {
            homeBorder = GameObject.Find("EastBordersOfKingdom");
            transform.parent.gameObject.tag = "Archer";
            if (homeBorder.transform.position.x <= -1000)
            {
                InvokeRepeating("WaitForBorderToExist", 0f, 5f);
            }
            else
            {
                ReturnToBorders();
            }
        }
    }

    private void NewWestBorder()
    {
        if (transform.parent.CompareTag("FreeArcher"))
        {
            homeBorder = GameObject.Find("WestBordersOfKingdom");
            transform.parent.gameObject.tag = "Archer";
            if (homeBorder.transform.position.x <= -1000)
            {
                InvokeRepeating("WaitForBorderToExist", 0f, 5f);
            }
            else
            {
                ReturnToBorders();
            }
        }
    }
}