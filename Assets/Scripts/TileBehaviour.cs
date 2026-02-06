using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> spritesList;
    [SerializeField] private GameObject bunny;
    private GameObject globalLight;

    private int actualNumBun;
    private bool bunCanSpawn;
    private float bunSpawnTimer;

    private int actualNumBuildings;
    private bool isMeadow;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        globalLight = GameObject.Find("GlobalLight2D");
        DayAndNightCycleBehaviour dayAndNightCycleBehaviour = globalLight.GetComponent<DayAndNightCycleBehaviour>();
        dayAndNightCycleBehaviour.OnChangeToSunRise += DayAndNightCycleBehaviourOnOnChangeToSunRise;
        dayAndNightCycleBehaviour.OnChangeToSunSet += DayAndNightCycleBehaviourOnOnChangeToSunSet;
        isMeadow = true;
        ChangeToMeadow();
    }

    private void DayAndNightCycleBehaviourOnOnChangeToSunSet(object sender, EventArgs e)
    {
        bunCanSpawn = false;
    }

    private void DayAndNightCycleBehaviourOnOnChangeToSunRise(object sender, EventArgs e)
    {
        bunCanSpawn = true;
    }


    void FixedUpdate()
    {
        bunSpawnTimer -= Time.fixedDeltaTime;
        SpawnBunnies();
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("MarkableTrees") || collider2D.CompareTag("MarkedTree") ||
            collider2D.CompareTag("BeggarsCamp"))
        {
            actualNumBuildings++;
            ChangeToForest();
        }
        else if (collider2D.CompareTag("Wall") || collider2D.CompareTag("MarkedWall") ||
                 collider2D.CompareTag("WallUnderAttack") || collider2D.CompareTag("TC") ||
                 collider2D.CompareTag("UpgradableTC") || collider2D.CompareTag("BowMarket") ||
                 collider2D.CompareTag("HammerMarket") || collider2D.CompareTag("OpenBowMarket") ||
                 collider2D.CompareTag("OpenHammerMarket"))
        {
            actualNumBuildings++;
            ChangeToMuddy();
        }
        else if (collider2D.CompareTag("Bunnies"))
        {
            actualNumBun++;   
            //Debug.Log("Bunny");
        }
    }


    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Bunnies"))
            actualNumBun--;
        else if (collider2D.CompareTag("MarkableTrees") || collider2D.CompareTag("MarkedTree") ||
                 collider2D.CompareTag("BeggarsCamp"))
        {
            actualNumBuildings--;
            if (actualNumBuildings <= 0)
            {
                ChangeToMeadow();
            }
        }
    }

    private void SpawnBunnies()
    {
        // if there is no night, bunnies can spawn
        if (isMeadow && bunCanSpawn && actualNumBun <= 2 && bunSpawnTimer <= 0)
        {
            // Instantiate(bunny, new Vector2(transform.position.x, 0.15f), Quaternion.identity);
            bunSpawnTimer = 60f;
        }
    }

    private void ChangeToForest()
    {
        // tile is forest
        isMeadow = false;
        spriteRenderer.sprite = spritesList[1];
    }

    private void ChangeToMeadow()
    {
        isMeadow = true;
        spriteRenderer.sprite = spritesList[2];
    }

    private void ChangeToMuddy()
    {
        isMeadow = false;
        spriteRenderer.sprite = spritesList[0];
    }
}
