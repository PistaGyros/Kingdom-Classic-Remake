using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxChest : MonoBehaviour
{
    private BoxCollider2D chestCollider;
    private SpriteRenderer chestSpriteRenderer;
    private GameObject townCenter;
    [SerializeField] private GameObject coin;
    private GameObject globalLight; 

    private int[] amountOfTaxCoins = new int[7] {0, 5, 6, 6, 7, 4, 1};
    private int townCenterLvl;
    private int amountOfSpawnedCoins;
    private bool chestIsReadyAgain;
    
    void Start()
    {
        chestCollider = GetComponent<BoxCollider2D>();
        chestCollider.enabled = false;
        chestSpriteRenderer = GetComponent<SpriteRenderer>();
        chestSpriteRenderer.enabled = false;
        townCenter = GameObject.Find("TownCenter");
        globalLight = GameObject.Find("GlobalLight2D");
        DayAndNightCycleBehaviour dayAndNightCycleBehaviour = globalLight.GetComponent<DayAndNightCycleBehaviour>();
        dayAndNightCycleBehaviour.OnChangeToNextDay += DayAndNightCycleBehaviourOnOnChangeToNextDay;
        amountOfSpawnedCoins = 0;
    }

    private void DayAndNightCycleBehaviourOnOnChangeToNextDay(object sender, DayAndNightCycleBehaviour.ChangeToNextDayArgs e)
    {
        ActivateYourself();
    }


    void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (chestIsReadyAgain && collider2D.CompareTag("Player"))
            PlayerHasCollided();
    }

    private void PlayerHasCollided()
    {
        Debug.Log("PLayer has collided with chest");
        chestIsReadyAgain = false;
        chestCollider.enabled = false;
        Invoke("Deactivate", 3f);
        InvokeRepeating("SpawnCoins", 0f, 0.2f);
    }

    private void Deactivate()
    {
        chestSpriteRenderer.enabled = false;
        // TODO: add some animation or so...
    }

    private void SpawnCoins()
    {
        if (amountOfSpawnedCoins <= amountOfTaxCoins[townCenterLvl])
        {
            amountOfSpawnedCoins++;
            Instantiate(coin, transform.position, Quaternion.identity);
        }
        else
        {
            CancelInvoke("SpawnCoins");
            amountOfSpawnedCoins = 0;
        }
    }

    private void ActivateYourself()
    {
        chestCollider.enabled = true;
        chestSpriteRenderer.enabled = true;
        chestIsReadyAgain = true;
        // townCenterLvl = townCenter.GetComponent<TownCenter>().actualLvl;
        // temporary solution
        townCenterLvl = 1;
    }
}
