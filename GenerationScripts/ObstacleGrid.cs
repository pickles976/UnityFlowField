using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Returns an "Obstacle Grid" --
/// a 2D array of integers with either NULL or MAXINT
/// NULL - empty space
/// MAXINT - impassable space
public class ObstacleGrid
{

    public static Tuple<Vector2,Vector2> getBounds(Vector2 bounds1, Vector2 bounds2){

        Vector2 bottomLeft = Vector2.zero;
        Vector2 topRight = Vector2.zero;

         // we want to get the top-right and bottom-left corners of a box given any bounds
        if (bounds1.x < bounds2.x){
            bottomLeft.x = bounds1.x;
            topRight.x = bounds2.x;
        }else{
            bottomLeft.x = bounds2.x;
            topRight.x = bounds1.x;
        }

        if (bounds1.y < bounds2.y){
            bottomLeft.y = bounds1.y;
            topRight.y = bounds2.y;
        }else{
            bottomLeft.y = bounds2.y;
            topRight.y = bounds1.y;
        }

        return new Tuple<Vector2,Vector2>(bottomLeft,topRight);
    }

    //This function will generate a 2D array of either NULL or MAXINT depending on whether there are
    //obstacles in the way or not.
    public static Dictionary<Tuple<int,int>,int> GenerateBlockedDictionary(CustomGrid cg, Vector3 bounds1, Vector3 bounds2)
    {

        float cellSize = cg.cellSize;

        RaycastHit hit; //raycast return variable
        GameObject go;	//keeping track of current game object

        // TODO: get rid of this magic number
        float radius = cellSize / 2; //radius of capsulecast

        Vector3 offset = (Vector3.right * 0.5f * cellSize) + (Vector3.forward * 0.5f * cellSize);

        Tuple<Vector2,Vector2> newBounds = getBounds(new Vector2(bounds1.x,bounds1.z),new Vector2(bounds2.x,bounds2.z));
        Vector2 bottomLeft = newBounds.Item1;
        Vector2 topRight = newBounds.Item2;

        // bounds in grid coordinates
        Tuple<int,int> b1 = cg.xzToCell(bottomLeft);
        Tuple<int,int> b2 = cg.xzToCell(topRight);

        int numCols = b2.Item1 - b1.Item1; // x length
        int numRows = b2.Item2 - b1.Item2; // z length

        Dictionary<Tuple<int,int>,int> obstacleGrid = new Dictionary<Tuple<int, int>, int>(); //create an empty grid to load obstacle values into
    
        // every column is X direction
        for (int col = b1.Item1; col < numCols; col++)
        {
            // every row is Z direction
            for (int row = b1.Item2; row < numRows; row++)
            {

                //create a new ray from 100 units up pointing down towards the map
                Ray ray = new Ray(cg.cellToWorld(row, col) + offset + (1000.0f * Vector3.up),Vector3.down);

                Vector3 origin = cg.cellToWorld(row, col) + offset + (1000.0f * Vector3.up);

                //check for obstacle
                if (Physics.SphereCast(origin, radius, Vector3.down,out hit, 50000.0f, (1 << 10)))
                {
                    //return the GameObject we hit
                    go = hit.transform.gameObject;
                    obstacleGrid[new Tuple<int, int>(row, col)] = Int32.MaxValue; //Max value represents impassable object
                }
            }
        }

            return obstacleGrid;

    }

}