using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmLand : MonoBehaviour
{
    [SerializeField] private GameObject coin;
    [SerializeField] private List<Sprite> phasesOfGrow;
    public GameObject itsFarmer;
    
    // components
    private SpriteRenderer spriteRendererFarmLand;

    // time remaining to grow to next phase of grow
    private float timeRemainingToGrow;
    private bool farmerIsPresent;
    public bool isOccupied;
    private bool cropsHaveGrown;
    private int spawnedCoins;
    
    private void Start()
    {
        timeRemainingToGrow = 140f;
        spriteRendererFarmLand = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (farmerIsPresent)
        {
            timeRemainingToGrow -= Time.fixedDeltaTime;
            ChangeItsVisual();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Farmer"))
            if (collider2D.gameObject == itsFarmer)
            {
                farmerIsPresent = true;
            }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Farmer"))
            if (collider2D.gameObject == itsFarmer)
            {
                farmerIsPresent = false;
            }
    }

    private void ChangeItsVisual()
    {
        if (timeRemainingToGrow is <= 120 and >= 118)
            spriteRendererFarmLand.sprite = phasesOfGrow[1];
        else if (timeRemainingToGrow is <= 100 and >= 98)
            spriteRendererFarmLand.sprite = phasesOfGrow[2];
        else if (timeRemainingToGrow is <= 80 and >= 78)
            spriteRendererFarmLand.sprite = phasesOfGrow[3];
        else if (timeRemainingToGrow is <= 60 and >= 58)
            spriteRendererFarmLand.sprite = phasesOfGrow[4];
        else if (timeRemainingToGrow is <= 40 and >= 38)
            spriteRendererFarmLand.sprite = phasesOfGrow[5];
        else if (timeRemainingToGrow is <= 20 and >= 18)
            spriteRendererFarmLand.sprite = phasesOfGrow[6];
        else if (timeRemainingToGrow is <= 0 and >= -2)
        {
            // crops have grown
            spriteRendererFarmLand.sprite = phasesOfGrow[7];
            cropsHaveGrown = true;
            if (cropsHaveGrown && spawnedCoins <= 3)
            {
                cropsHaveGrown = false;
                InvokeRepeating("SpawnCoins", 0f, 0.5f);
            }
            else
            {
                CancelInvoke("SpawnCoins");
            }
        }
    }

    private void SpawnCoins()
    {
        Instantiate(coin, new Vector2(transform.position.x, 4), Quaternion.identity);
        spawnedCoins++;
    }
}
