using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : MonoBehaviour
{
    [SerializeField] private GameObject farmLandPrefab;
    private GameObject itsField;

    public bool hasAssignedField;
    private float newFarmCoords;
    
    
    
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Farm"))
        {
            
        }
        if (collider2D.CompareTag("Ground"))
        {
            if (hasAssignedField)
            {
                if (newFarmCoords == collider2D.transform.position.x)
                {
                    // yeey we can start growing stuff
                    CreateNewField();
                }
            }
        }
    }

    public void GoToYourField(float newFarmCoords)
    {
        
    }

    private void CreateNewField()
    {
        itsField = Instantiate(farmLandPrefab, new Vector2(newFarmCoords, 0.35f), Quaternion.identity);
        itsField.GetComponent<FarmLand>().isOccupied = true;
        itsField.GetComponent<FarmLand>().itsFarmer = gameObject;
    }
}
