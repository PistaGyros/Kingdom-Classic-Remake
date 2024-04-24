using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class Arrow : MonoBehaviour
{
    private GameObject target;
    private BoxCollider2D boxCollider2D;
    [SerializeField] GameObject archer;
    [SerializeField] GameObject arrowShooter;
    private Vector3 positionOfTarget;
    public float speed = 10f;
    public float launchHeight = 1.5f;
    public Vector3 movePosition;
    private float startX;
    private float targetX;
    private float nextX;
    private float dist;
    private float baseY;
    private float height;
    public float timeLeft;

    private Vector2 originalCords;
    public bool shooted = true;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        startX = transform.position.x;
        timeLeft = 2.9f;
    }

    private void Update()
    {
        if (shooted)
        {
            positionOfTarget = arrowShooter.GetComponent<ArrowShooter>().targetPos;
            targetX = positionOfTarget.x;
            Vector3 position = transform.position;
            dist = targetX - startX;
            nextX = Mathf.MoveTowards(position.x, targetX, speed * Time.deltaTime);
            baseY = Mathf.Lerp(position.y, positionOfTarget.y, (nextX - startX) / dist);
            height = launchHeight * (nextX - startX) * (nextX - targetX) / (-0.25f * dist * dist);
            movePosition = new Vector3(nextX, baseY + height, position.z);
            transform.rotation = LookAtTarget(movePosition - position);
            transform.position = movePosition;
        }
    }

    private void FixedUpdate()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            transform.position = archer.transform.position;
            gameObject.SetActive(false);
            boxCollider2D.enabled = true;
        }
        else if (transform.position == positionOfTarget)
        {
            shooted = false;
            boxCollider2D.enabled = false;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Bunnies"))
        {
            timeLeft = -1f;
        }
        else if (collider2D.CompareTag("TrollCollider") || collider2D.CompareTag("DeadTroll"))
        {
            if (shooted)
            {
                shooted = false;
                boxCollider2D.enabled = false;
                Destroy(collider2D.transform.parent.gameObject, 0.75f);
                timeLeft = -1f;   
            }
        }
    }

    public static Quaternion LookAtTarget(Vector2 r)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
    }
}