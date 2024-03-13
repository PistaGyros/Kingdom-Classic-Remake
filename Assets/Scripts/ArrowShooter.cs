using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    public Vector2 targetPos;
    public bool shoot;

    private float timeToShootAgain = 5f;
    private bool canShoot = false;

    void Start()
    {
        
    }

    void Update()
    {
        timeToShootAgain += Time.deltaTime;
        if (timeToShootAgain >= 3f)
            canShoot = true;
    }

    void OnTriggerStay2D(Collider2D collider2D)
    {
        if (canShoot && collider2D.CompareTag("Bunnies"))
        {
            canShoot = false;
            timeToShootAgain = 0;
            targetPos = collider2D.transform.position;
            arrow.SetActive(true);
        }
        if (canShoot && collider2D.CompareTag("Enemies"))
        {
            canShoot = false;
            timeToShootAgain = 0;
            targetPos = collider2D.transform.position;
            arrow.SetActive(true);
        }
    }
}