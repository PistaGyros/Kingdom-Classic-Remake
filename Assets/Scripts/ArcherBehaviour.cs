using System;
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

    private bool borderDoesExist;
    private bool isAtHisStation;
    

    void Start()
    {
        archerRigidbody2D = gameObject.GetComponentInParent<Rigidbody2D>();
        archerAnimator = gameObject.GetComponentInParent<Animator>();
        canPickUp = true;
        // init of events
        //GeneralHandler generalHandler = globalLight.GetComponent<GeneralHandler>(); ... remove if no more reliable
        globalLight = GameObject.Find("GlobalLight2D");
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
        if (borderDoesExist)
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
            isAtHisStation = true;
            archerDirection = 0;
            archerAnimator.SetBool("IsWalking", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("PositionForArchers"))
            isAtHisStation = false;
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
            CancelInvoke("DropCoin");
    }

    private void StartWander()
    {
        //TODO: add this
    }

    private void ReturnToBorders()
    {
        if (!isAtHisStation)
        {
            if (transform.position.x < homeBorder.transform.position.x)
                archerDirection = 1;
            else
                archerDirection = -1;
            gameObject.transform.parent.localScale = new Vector2(archerDirection, 1);
            archerAnimator.SetBool("IsWalking", true);
        }
    }

    private void Run()
    {
        Vector2 archerVelocity = new Vector2(archerDirection, 0);
        archerRigidbody2D.velocity = archerVelocity * archerSpeed;
    }

    private void WaitForBorderToExist()
    {
        if (homeBorder.transform.position.x > -10000)
        {
            borderDoesExist = true;
            CancelInvoke("WaitForBorderToExist");
            ReturnToBorders();
        }
    }

    private void NewEastBorder()
    {
        if (transform.parent.CompareTag("FreeArcher"))
        {
            transform.parent.gameObject.tag = "Archer";
            homeBorder = GameObject.Find("EastBordersOfKingdom");
            BorderOfKingdom borderOfKingdom = homeBorder.GetComponent<BorderOfKingdom>();
            borderOfKingdom.OnPosOfBorderHasChanged += BorderOfKingdomOnOnPosOfBorderHasChanged;
            DoesBorderExist();
        }
    }

    private void NewWestBorder()
    {
        if (transform.parent.CompareTag("FreeArcher"))
        {
            transform.parent.gameObject.tag = "Archer";
            homeBorder = GameObject.Find("WestBordersOfKingdom");
            BorderOfKingdom borderOfKingdom = homeBorder.GetComponent<BorderOfKingdom>();
            borderOfKingdom.OnPosOfBorderHasChanged += BorderOfKingdomOnOnPosOfBorderHasChanged;
            DoesBorderExist();
        }
    }

    private void DoesBorderExist()
    {
        if (homeBorder.transform.position.x <= -1000)
        {
            InvokeRepeating("WaitForBorderToExist", 0f, 5f);
            borderDoesExist = false;
        }
        else
        {
            borderDoesExist = true;
            ReturnToBorders();
        }
    }
}