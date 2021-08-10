using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// An Object with utility functions that help
/// with discretizing world space
public class CustomGrid
{

    //grid size

    public float cellSize;

    public CustomGrid(float cellSize){
        this.cellSize = cellSize;
    }

    //transforms world position into grid indices
    public Tuple<int,int> worldToCell(Vector3 worldPos)
    {
       int xCell = (int)Mathf.Floor(worldPos.x / cellSize);
       int zCell = (int)Mathf.Floor(worldPos.z / cellSize);

       return new Tuple<int,int>(xCell,zCell);
    }

    //transforms world pos to grid indices
    public Tuple<int,int> xzToCell(Vector2 worldPos)
    {
       int xCell = (int)Mathf.Floor(worldPos.x / cellSize);
       int zCell = (int)Mathf.Floor(worldPos.y / cellSize);

       return new Tuple<int,int>(xCell,zCell);
    }

    //turns grid indices into approximate world coordinates
    public Vector3 cellToWorld(int xCell,int zCell)
    {
        Vector3 vector = new Vector3(0.0f,0.0f,0.0f);

        vector.x = xCell * cellSize;
        vector.z = zCell * cellSize;

        return vector;
    }

    //turns grid indices into approximate world coordinates
    public Vector3 tupleToWorld(Tuple<int,int> pair)
    {
        Vector3 vector = new Vector3(0.0f,0.0f,0.0f);

        vector.x = pair.Item1 * cellSize;
        vector.z = pair.Item2 * cellSize;

        return vector;
    }

}