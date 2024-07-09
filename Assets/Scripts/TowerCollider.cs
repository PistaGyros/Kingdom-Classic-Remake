using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCollider : MonoBehaviour
{
    [SerializeField] private GameObject archerTower;
    
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
            archerTower.GetComponent<ArcherTower>().playerHasCollided = true;
        else if (collider2D.CompareTag("Builder"))
        {
            archerTower.GetComponent<ArcherTower>().activeWorkers.Add(collider2D.gameObject);
            archerTower.GetComponent<ArcherTower>().BuilderHasCollided();
        }
        else if (collider2D.CompareTag("ArcherCollider"))
        {
            if (archerTower.GetComponent<ArcherTower>().numberOfArchers <
                archerTower.GetComponent<ArcherTower>().numeroOfAllowedArchers)
            {
                collider2D.transform.parent.position = 
                    archerTower.GetComponent<ArcherTower>().SetPositionsForArchersStandPoints();
                int sortingOrder = archerTower.GetComponent<ArcherTower>().numberOfArchers == 3 ? -4 : -2;
                collider2D.GetComponentInParent<SpriteRenderer>().sortingOrder = sortingOrder;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
            archerTower.GetComponent<ArcherTower>().playerHasCollided = false;
        else if (collider2D.CompareTag("Builder"))
        {
            archerTower.GetComponent<ArcherTower>().activeWorkers.Remove(collider2D.gameObject);
            archerTower.GetComponent<ArcherTower>().BuilderHasLeft();
        }
    }
}