using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BorderOfKingdom : MonoBehaviour
{
    private Vector2 actualPosition;
    private GameObject[] walls;
    private List<GameObject> listOfWalls = new List<GameObject>();
    private Vector2 positionOfCalledWall;

    private bool isEast;
    

    void Start()
    {
        if (transform.tag == "EastBorder")
            isEast = true;
        else
        {
            isEast = false;
            transform.localScale = new Vector2(-1, 1);
        }

        walls = GameObject.FindGameObjectsWithTag("EmptyWall");
        foreach (GameObject wall in walls)
        {
            Wall wall1 = wall.GetComponent<Wall>();
            wall1.WallHasBeenUpgraded += Wall1OnWallHasBeenUpgraded;
        }
    }

    private void Wall1OnWallHasBeenUpgraded(object sender, Wall.CallToWallArgs e)
    {
        positionOfCalledWall = e.positionOfWall;
        if (isEast && positionOfCalledWall.x > 0)
            ChangePositionOfBorders();
        else if (!isEast && positionOfCalledWall.x < 0)
            ChangePositionOfBorders();
    }
    
    private void ChangePositionOfBorders()
    {
        walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject wall in walls)
        {
            listOfWalls.Add(wall);
        }
        listOfWalls.OrderBy(wall => wall.transform.position.x);
        // here put some animation and stuff
        if (isEast)
            transform.position = new Vector2(listOfWalls[0].transform.position.x + 2.5f, transform.position.y);
        else
            transform.position = new Vector2(listOfWalls[^1].transform.position.x - 2.5f, transform.position.y);
    }
}
