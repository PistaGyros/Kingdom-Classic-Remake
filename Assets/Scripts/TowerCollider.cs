using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCollider : MonoBehaviour
{
    [SerializeField] private GameObject archerTower;
    private ArcherTower archerTowerCode;

    private void Start()
    {
        archerTowerCode = archerTower.GetComponent<ArcherTower>();
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
            archerTowerCode.playerHasCollided = true;
        else if (collider2D.CompareTag("Builder"))
        {
            archerTowerCode.activeWorkers.Add(collider2D.gameObject);
            archerTowerCode.BuilderHasCollided();
        }
        else if (collider2D.CompareTag("ArcherCollider"))
        {
            if (archerTowerCode.numberOfArchers <
                archerTowerCode.numeroOfAllowedArchers)
            {
                collider2D.transform.parent.position = 
                    archerTowerCode.SetPositionsForArchersStandPoints();
                int sortingOrder = archerTowerCode.numberOfArchers == 3 ? -4 : -2;
                collider2D.GetComponentInParent<SpriteRenderer>().sortingOrder = sortingOrder;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
            archerTowerCode.playerHasCollided = false;
        else if (collider2D.CompareTag("Builder"))
        {
            archerTowerCode.activeWorkers.Remove(collider2D.gameObject);
            archerTowerCode.BuilderHasLeft();
        }
    }
}
