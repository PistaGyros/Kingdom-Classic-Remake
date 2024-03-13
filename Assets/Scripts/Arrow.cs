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
    [SerializeField] GameObject archer;
    [SerializeField] GameObject arrowShooter;
    private Vector2 positionOfTarget;
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

    private Vector2 originalCords;

    private void Start()
    {
        originalCords = gameObject.transform.parent.position;
        positionOfTarget = arrowShooter.GetComponent<ArrowShooter>().targetPos;
        targetX = positionOfTarget.x;
        startX = transform.position.x;        
        timeLeft = 2.9f;
    }

    private void Update()
    {
        dist = targetX - startX;
        nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        baseY = Mathf.Lerp(transform.position.y, positionOfTarget.y, (nextX - startX) / dist);
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
            transform.position = originalCords;
            gameObject.SetActive(false);
        }
    }

    public static Quaternion LookAtTarget(Vector2 r)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
    }
}