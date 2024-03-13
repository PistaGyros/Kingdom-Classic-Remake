using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BeggarsCamps : MonoBehaviour
{
    [SerializeField] GameObject beggar;

    public int numberOfBeggars;

    private float spawnTimer = 120f;

    System.Random rnd = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        SpawnBeggar();
        Invoke("SpawnBeggar", 2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0 && numberOfBeggars <= 2)
        {
            SpawnBeggar();   
            spawnTimer = 120f;            
        }
    }

    private void SpawnBeggar()
    {
        Instantiate(beggar, new Vector2(rnd.Next((int)transform.position.x - 3, (int)transform.position.x + 3), 0.4f), Quaternion.identity);
        numberOfBeggars++;
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
