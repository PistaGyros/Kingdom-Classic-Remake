using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.Unicode;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class TrollCollider : MonoBehaviour
{
    private int hitPoints = 1;

    private int direction;

    private Rigidbody2D rigidbody2D;


    private void Start()
    {
        rigidbody2D = GetComponentInParent<Rigidbody2D>();
        if (transform.position.x > 0)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
    }

    private void FixedUpdate()
    {
        if (hitPoints <= 0)
            Destroy(transform.parent.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision2D)
    {
        if (collision2D.CompareTag("Arrow"))
        {
            hitPoints -= 1;
        }
        else if (collision2D.CompareTag("Wall") || collision2D.CompareTag("WallUnderAttack"))
        {
            Debug.Log("Troll has collided");
            BackOff();
        }
        else if (collision2D.CompareTag("Ground"))
        {
            //rigidbody2D.velocity = new Vector2(0, 0);
        }
    }

    private void BackOff()
    {
        rigidbody2D.velocity = new Vector2(0, 0);
        rigidbody2D.velocity += new Vector2(-5f * direction, 1.5f);
    }
}