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
    

    void Start()
    {
        walls = GameObject.FindGameObjectsWithTag("EmptyWall");
        foreach (GameObject wall in walls)
        {
            Wall wall1 = wall.GetComponent<Wall>();
            wall1.WallHasBeenUpgraded += Wall1OnWallHasBeenUpgraded;
        }

    }

    private void Wall1OnWallHasBeenUpgraded(object sender, Wall.CallToWallArgs e)
    {
        ChangePositionOfBorders();
    }
    // finish this shit
    private void ChangePositionOfBorders()
    {
        walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject wall in walls)
        {
            listOfWalls.Add(wall);
        }
        listOfWalls.OrderBy(wall => wall.transform.position.x);
        
        // here put some animation and stuff
        transform.position = new Vector2(listOfWalls[^1].transform.position.x + 5, transform.position.y);
    }
}
