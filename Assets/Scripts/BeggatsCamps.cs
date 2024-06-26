using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BeggarsCamps : MonoBehaviour
{
    [SerializeField] GameObject beggar;
    public int numberOfBeggars;
    private float spawnTimer = 120f;
    private Vector2 pos;

    System.Random rnd = new System.Random();
    
    void Start()
    {
        pos = transform.position;
        SpawnBeggar();
        Invoke("SpawnBeggar", 3f);
    }
    
    void FixedUpdate()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0 && numberOfBeggars <= 2)
        {
            SpawnBeggar();   
            spawnTimer = 120f;
        }
    }

    private void SpawnBeggar()
    {
        int r = rnd.Next((int)pos.x - 3, (int)pos.x + 3);
        Instantiate(beggar, new Vector2(r, 0.37f), Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Beggar"))
        {
            numberOfBeggars++;
        }
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Beggar"))
        {
            numberOfBeggars--;
        }
    }
}
