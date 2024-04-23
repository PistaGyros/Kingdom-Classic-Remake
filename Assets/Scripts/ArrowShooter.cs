using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    public Vector3 targetPos;
    public bool shoot;
    private List<Collider2D> bunnyTargetList = new List<Collider2D>();
    private List<Collider2D> enemiesTargetList = new List<Collider2D>();

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
        if (canShoot && enemiesTargetList.Count >= 1)
        {
            Shoot(enemiesTargetList[0]);
        }
        else if (canShoot && bunnyTargetList.Count >= 1)
        {
            Shoot(bunnyTargetList[0]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Bunnies"))
        {
            if (bunnyTargetList.IndexOf(collider2D) == -1)
            {
                bunnyTargetList.Add(collider2D);
            }
        }
        else if (collider2D.CompareTag("TrollCollider"))
        {
            if (enemiesTargetList.IndexOf(collider2D) == -1)
            {
                enemiesTargetList.Add(collider2D);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("DeadBunny") || collider2D.CompareTag("Bunnies"))
        {
            bunnyTargetList.Remove(collider2D);
        }
        else if (collider2D.CompareTag("DeadTroll") || collider2D.CompareTag("TrollCollider"))
        {
            enemiesTargetList.Remove(collider2D);
        }
    }

    private void Shoot(Collider2D target)
    {
        canShoot = false;
        timeToShootAgain = 0;
        if (target != null)
            targetPos = target.transform.position;
        Invoke("Shooting", 1f);
    }

    private void Shooting()
    {
        arrow.SetActive(true);
        arrow.GetComponent<Arrow>().shooted = true;
        arrow.GetComponent<Arrow>().timeLeft = 2.9f;
    }
}