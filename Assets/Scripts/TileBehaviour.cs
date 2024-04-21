using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite muddyTile;
    [SerializeField] private Sprite meadowTile;
    [SerializeField] private Sprite forestTile;
    private List<Sprite> spritesList = new List<Sprite>();
    [SerializeField] private GameObject bunny;
    private GameObject globalLight;

    private int actualNumBun;
    private bool bunCanSpawn;
    private float bunSpawnTimer;

    private int actualNumTrees;
    private bool isMeadow;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spritesList.Add(muddyTile);
        spritesList.Add(meadowTile);
        spritesList.Add(forestTile);
        globalLight = GameObject.Find("GlobalLight2D");
        DayAndNightCycleBehaviour dayAndNightCycleBehaviour = globalLight.GetComponent<DayAndNightCycleBehaviour>();
        dayAndNightCycleBehaviour.OnChangeToSunRise += DayAndNightCycleBehaviourOnOnChangeToSunRise;
        dayAndNightCycleBehaviour.OnChangeToSunSet += DayAndNightCycleBehaviourOnOnChangeToSunSet;
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
        if (collider2D.CompareTag("MarkableTrees") || collider2D.CompareTag("MarkedTree"))
        {
            // tile is forest
            isMeadow = false;
            actualNumTrees++;
            spriteRenderer.sprite = spritesList[2];
        }
        else if (collider2D.CompareTag("Wall") || collider2D.CompareTag("MarkedWall") || 
                 collider2D.CompareTag("WallUnderAttack"))
        {
            // tile is in the city, with no grass
            isMeadow = false;
            spriteRenderer.sprite = spritesList[0];
        }
        else if (collider2D.CompareTag("TownCenter1") || collider2D.CompareTag("BowMarket") ||
                 collider2D.CompareTag("HammerMarket") || collider2D.CompareTag("OpenBowMarket") || 
                 collider2D.CompareTag("OpenHammerMarket"))
        {
             // tile is in the city, grass can spawn
             isMeadow = false;
             spriteRenderer.sprite = spritesList[0];
        }
        else if (collider2D.CompareTag("Bunnies"))
        {
            actualNumBun++;   
            Debug.Log("Bunny");
        }
    }


    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Bunnies"))
            actualNumBun--;
        else if (collider2D.CompareTag("MarkableTrees") || collider2D.CompareTag("MarkedTree"))
        {
            actualNumTrees--;
            if (actualNumTrees <= 0)
            {
                // tile is meadow
                isMeadow = true;
                spriteRenderer.sprite = spritesList[1];
            }
        }
    }

    private void SpawnBunnies()
    {
        // if there is no night, bunnies can spawn
        if (bunCanSpawn && actualNumBun <= 2 && bunSpawnTimer <= 0)
        {
            Instantiate(bunny, new Vector2(transform.position.x, 0.15f), Quaternion.identity);
            bunSpawnTimer = 60f;
        }
    }
}
