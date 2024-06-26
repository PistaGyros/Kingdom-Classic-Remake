using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeasentPickUp : MonoBehaviour
{
    [SerializeField] GameObject archer;
    [SerializeField] GameObject builder;
    [SerializeField] GameObject farmer;
    [SerializeField] GameObject hammerMarket;
    [SerializeField] GameObject bowMarket;
    private float peasentSpeed = 5f;
    private Rigidbody2D rigidbody2D;
    private Animator animatorPeasent;
    private float direction;
    private int numberOfCoinsOfPeasent;


    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animatorPeasent = GetComponent<Animator>();
        hammerMarket = GameObject.Find("HammerMarket");
        HammerMarketZero callPeasentToHammer = hammerMarket.GetComponent<HammerMarketZero>();
        callPeasentToHammer.OnCallPeasent += CallPeasent_OnCallPeasent;
        callPeasentToHammer.OnStopCallPeasent += CallPeasent_OnStopCallPeasent;
        bowMarket = GameObject.Find("BowMarket");
        BowMarketZero callPeasentToBow = bowMarket.GetComponent<BowMarketZero>();
        callPeasentToBow.OnCallPeasent += CallPeasentToBow_OnCallPeasent;
        callPeasentToBow.OnStopCallPeasent += CallPeasentToBow_OnStopCallPeasent;
        RunToKingdomCenter();
    }

    private void CallPeasentToBow_OnStopCallPeasent(object sender, System.EventArgs e)
    {
        if (this != null)
        {
            direction = 0;
            animatorPeasent.SetBool("IsMoving", false);
        }
    }

    private void CallPeasentToBow_OnCallPeasent(object sender, System.EventArgs e)
    {
        if (this != null)
            RunForBow();           
    }

    private void CallPeasent_OnStopCallPeasent(object sender, System.EventArgs e)
    {
        if (this != null)
        {
            direction = 0;
            animatorPeasent.SetBool("IsMoving", false);
        }
    }

    private void CallPeasent_OnCallPeasent(object sender, System.EventArgs e)
    {
        if (this != null)
            RunForHammer();          
    }

    void FixedUpdate()
    {
        Run();
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        //After it picks up a tool, it gets a new profession
        if (collider2D.CompareTag("OpenBowMarket"))
        {
            Instantiate(archer, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            Destroy(gameObject, 0f);
        }
        else if (collider2D.gameObject.CompareTag("OpenHammerMarket"))
        {
            Instantiate(builder, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            Destroy(gameObject, 0f);
        }
        /*else if (collider2D.gameObject.CompareTag("ScytheMarket"))
        {
            Instantiate(farmer, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        }*/
        else if (collider2D.gameObject.CompareTag("Coins") || collider2D.gameObject.CompareTag("PlayerCoins"))
        {
            numberOfCoinsOfPeasent += 1;
        }
        else if (collider2D.CompareTag("TownCenter1"))
        {
            direction = 0;
            animatorPeasent.SetBool("IsMoving", false);
        }
    }


    private void Run()
    {
        Vector2 peasentVelocity = new Vector2(direction, 0);
        rigidbody2D.velocity = peasentVelocity * peasentSpeed;
    }

    private void RunToKingdomCenter()
    {
        direction = transform.position.x > 0 ? -1 : 1;
        animatorPeasent.SetBool("IsMoving", true);
    }

    private void RunForHammer()
    {
        if (hammerMarket != null)
            direction = transform.position.x > hammerMarket.transform.position.x ? -1 : 1;
        animatorPeasent.SetBool("IsMoving", true);
        transform.localScale = new Vector2(direction, 1);
    }

    private void RunForBow()
    {
        if (bowMarket != null)
            direction = transform.position.x > bowMarket.transform.position.x ? -1 : 1;
        animatorPeasent.SetBool("IsMoving", true);
        transform.localScale = new Vector2(direction, 1);
    }
}