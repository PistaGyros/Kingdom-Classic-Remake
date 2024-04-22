using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    public Vector3 targetPos;
    public bool shoot;

    private float timeToShootAgain = 5f;
    private bool canShoot = false;

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        timeToShootAgain += Time.fixedDeltaTime;
        if (timeToShootAgain >= 3f)
            canShoot = true;
    }

    private void OnTriggerStay2D(Collider2D collider2D)
    {
        if (canShoot && collider2D.CompareTag("Bunnies"))
        {
            Shoot(collider2D);
        }
        if (canShoot && collider2D.CompareTag("TrollCollider"))
        {
            Shoot(collider2D);
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Bunnies"))
        {
            CancelInvoke("Delay");
            arrow.GetComponent<Arrow>().timeLeft = -1;
        }
    }

    private void Shoot(Collider2D collider2D)
    {
        canShoot = false;
        timeToShootAgain = 0;
        targetPos = collider2D.transform.position;
        Invoke("Delay", 1f);
    }

    private void Delay()
    {
        arrow.SetActive(true);
        arrow.GetComponent<Arrow>().shooted = true;
        arrow.GetComponent<Arrow>().timeLeft = 2.9f;
    }
}