using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private BoxCollider2D farmBoxCollider;

    private int farmLevel;
    
    private bool isFarmActive;

    private void Start()
    {
        player = GameObject.Find("Player");
        farmBoxCollider.enabled = false;
    }

    private void FixedUpdate()
    {
        
    }

    public void ActivateFarm()
    {
        isFarmActive = true;
        farmBoxCollider.enabled = true;
    }
}
