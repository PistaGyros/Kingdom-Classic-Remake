using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : MonoBehaviour
{
    public GameObject itsField;
    private Rigidbody2D farmersRigidbody2D;
    private static string freeFarmer = "FreeFarmer";
    private int direction = 0;
    private float speed = 1;
    
    
    void Start()
    {
        transform.tag = freeFarmer;
        farmersRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        FarmerMove();
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("OpenFarm"))
        {
            transform.tag = "Farmer";
        }
    }

    public void GoToYourFarm(float newFarmCoords)
    {
        if (transform.position.x < newFarmCoords)
            direction = 1;
        else
            direction = -1;
    }

    public void GoToYourNewFarmLand(GameObject farmLand)
    {
        itsField = farmLand;
        if (transform.position.x < itsField.transform.position.x)
            direction = 1;
        else
            direction = -1;
    }

    public void GoToYourAssignedField()
    {
        if (transform.position.x < itsField.transform.position.x)
            direction = 1;
        else
            direction = -1;
    }

    private void FarmerMove()
    {
        Vector2 farmerVelocity = new Vector2(direction, 0);
        farmersRigidbody2D.velocity = farmerVelocity * speed;
    }
}
