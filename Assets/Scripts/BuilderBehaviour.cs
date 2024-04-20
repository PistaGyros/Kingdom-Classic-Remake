using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class BuilderBehaviour : MonoBehaviour
{
    [SerializeField] float builderSpeed;
    private GameObject[] umarkedTrees;
    private GameObject[] markedTrees;
    private GameObject[] walls;
    private GameObject[] markedWalls;
    private GameObject[] wallsUnderAttack;
    private GameObject[] emptyWalls;
    Rigidbody2D rigidbody2D;
    private float direction;
    int numberOfCoinsOfBuilder;
    bool isBussy;
    private bool runForWall = false;
    private Vector2 targetPos;
    private float sinTime;
    private Vector2 currentPos;

    // add coin drop mechanic when player comes to the builder
    // maybe add priority mechanic

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        if (umarkedTrees == null)
            umarkedTrees = GameObject.FindGameObjectsWithTag("MarkableTrees");
        if (markedTrees == null)
            markedTrees = GameObject.FindGameObjectsWithTag("MarkedTree");
        foreach (GameObject tree in umarkedTrees) 
        {
            Tree1 tree1 = tree.GetComponent<Tree1>();
            tree1.OnCallToBuildersToTree += Tree1_OnCallToBuildersToTree;
            tree1.OnStopCallToBuildersToTree += Tree1_OnStopCallToBuildersToTree;
        }
        foreach(GameObject tree in markedTrees)
        {
            Tree1 tree1 = tree.GetComponent<Tree1>();
            tree1.OnCallToBuildersToTree += Tree1_OnCallToBuildersToTree;
            tree1.OnStopCallToBuildersToTree += Tree1_OnStopCallToBuildersToTree;
        }
        if (walls == null)
            walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach(GameObject wall in walls)
        {
            Wall walls = wall.GetComponent<Wall>();
            walls.OnCallBuilderToWall += Walls_OnCallBuilderToWall;
            walls.OnStopCallBuilderToWall += Walls_OnStopCallBuilderToWall;
        }
        if (markedWalls == null)
            markedWalls = GameObject.FindGameObjectsWithTag("MarkedWall");
        foreach (GameObject wall in markedWalls)
        {
            Wall walls = wall.GetComponent<Wall>();
            walls.OnCallBuilderToWall += Walls_OnCallBuilderToWall;
            walls.OnStopCallBuilderToWall += Walls_OnStopCallBuilderToWall;
        }
        if (wallsUnderAttack == null)
            wallsUnderAttack = GameObject.FindGameObjectsWithTag("WallUnderAttack");
        foreach( GameObject wall in wallsUnderAttack)
        {
            Wall walls = wall.GetComponent<Wall>();
            walls.OnCallBuilderToWall += Walls_OnCallBuilderToWall;
            walls.OnStopCallBuilderToWall += Walls_OnStopCallBuilderToWall;
        }

        if (emptyWalls == null)
            emptyWalls = GameObject.FindGameObjectsWithTag("EmptyWall"); ;
        foreach( GameObject wall in emptyWalls)
        {
            Wall walls = wall.GetComponent<Wall>();
            walls.OnCallBuilderToWall += Walls_OnCallBuilderToWall;
            walls.OnStopCallBuilderToWall += Walls_OnStopCallBuilderToWall;
        }
    }


    private void Walls_OnCallBuilderToWall(object sender, Wall.CallToWallArgs e)
    {
        currentPos = transform.position;
        targetPos = new Vector2(e.positionOfWall.x, transform.position.y);
        runForWall = true;
        isBussy = true;
    }
    private void Walls_OnStopCallBuilderToWall(object sender, System.EventArgs e)
    {
        runForWall = false;
        isBussy = false;
    }
    private void Tree1_OnCallToBuildersToTree(object sender, Tree1.CallToBuilderArgs e)
    {
        isBussy = true;
        if (e.positionOfTree.x > transform.position.x && !isBussy)
            direction = 1;
        else if(!isBussy)
            direction = -1;
    }
    private void Tree1_OnStopCallToBuildersToTree(object sender, System.EventArgs e)
    {
        direction = 0;
        isBussy = false;
    }

    void FixedUpdate()
    {
        RunForMarkedTree();
        RunForWall();
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("MarkedTree") || collider2D.CompareTag("MarkedWall") || collider2D.CompareTag("WallUnderAttack"))
        {
            isBussy = true;
            direction = 0;
        }
        else if (collider2D.CompareTag("Coins") || collider2D.CompareTag("PlayerCoins"))
        {
            numberOfCoinsOfBuilder += 1;
        }
    }


    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("MarkedTree") || collider2D.CompareTag("MarkedWall"))
        {
            isBussy = false;
        }
    }

    private void RunForMarkedTree()
    {
        Vector2 builderVelocity = new Vector2(direction, 0);
        rigidbody2D.velocity = builderVelocity * builderSpeed;
    }

    private void RunForWall()
    {
        if (runForWall)
        {
            sinTime += Time.fixedDeltaTime * 1;//builder speed
            sinTime = Mathf.Clamp(sinTime, 0, Mathf.PI);
            float t = evaluate(sinTime);
            transform.position = Vector2.Lerp(currentPos, targetPos, t);
        }
    }

    public float evaluate(float x)
    {
        return 0.5f * Mathf.Sin(x - Mathf.PI / 2) + 0.5f;
    }
}