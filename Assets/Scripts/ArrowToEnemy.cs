using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowToEnemy : MonoBehaviour
{
    public GameObject target;
    public float speed = 10f;
    public float launchHeight = 2f;
    public Vector3 movePosition;
    private float startX;
    private float targetX;
    private float nextX;
    private float dist;
    private float baseY;
    private float height;

    private float timeLeft;

    void Start()
    {
        startX = transform.position.x;
        target = FindClosestTarget();
        targetX = target.transform.position.x;
        timeLeft = 5f;
    }

    void Update()
    {
        dist = targetX - startX;
        nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        baseY = Mathf.Lerp(transform.position.y, target.transform.position.y, (nextX - startX) / dist);
        height = launchHeight * (nextX - startX) * (nextX - targetX) / (-0.25f * dist * dist);
        movePosition = new Vector3(nextX, baseY + height, transform.position.z);
        transform.rotation = LookAtTarget(movePosition - transform.position);
        transform.position = movePosition;
    }

    private void FixedUpdate()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) 
        {
            Destroy(gameObject, 0f);
        }
    }

    public static Quaternion LookAtTarget(Vector2 r)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
    }

    //Find the closest enemy
    public GameObject FindClosestTarget()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemies");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
