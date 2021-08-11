using System;
using System.Collections.Generic;
using UnityEngine;

/// Script used to access the static FlowFieldProvider class
public class FlowFieldController : MonoBehaviour
{

    [Header("Field generation settings")]
    // determines whether or not we will re-check for obstacles
    public bool dynamicObstacles;

    public float cellSize;

    // TODO: BUG, X AND Z ARE FLIPPED (HOW DID THIS HAPPEN???)
    public Vector3 b1;
    public Vector3 b2;

    public int obstacleLayer;

    public GameObject player;

    [Header("Debug Options")]
    public bool showObstacles;
    public bool showDijkstra;
    public bool showVectors;

    // Dictionary linking grid coordinates to passability values
    Dictionary<Tuple<int,int>,int> obstacles;

    // Dictionary linking grid coordinates to Vector3s of FlowField
    Dictionary<Tuple<int,int>,Vector3> ff;

    // DijkstraGrid
    int[,] dGrid;

    // Utility used for transforming between world and grid coordinates
    CustomGrid cg;

    [Header("Resource Allocation")]
    public float refreshRate;
    float timer;

    // TODO: will be used for Async processing, not used at all right now
    public int cellsPerFrame;
    public int obstacleChecksPerFrame;

    // Start is called before the first frame update
    void Awake()
    {
        timer = refreshRate;

        cg = new CustomGrid(cellSize);

        //generate initial obstacle grid
        obstacles = ObstacleGrid.GenerateBlockedDictionary(cg,b1,b2,obstacleLayer);
        GenerateNewField(Vector3.zero,b1,b2);
    }

    void LateUpdate(){
        if(timer > 0){
            timer -= Time.deltaTime;
        }else{
            timer = refreshRate;
            GenerateNewField(player.transform.position,b1,b2);
        }
    }

    // Generate a new FlowField from destination and bounds
    void GenerateNewField(Vector3 destination, Vector3 bound1, Vector3 bound2){
        if(dynamicObstacles){
            obstacles = ObstacleGrid.GenerateBlockedDictionary(cg,bound1,bound2,obstacleLayer);
        }
        FlowFieldProvider.GenerateNewField(cg,obstacles,bound1,bound2,destination,cellsPerFrame);
        ff = FlowFieldProvider.GetField();
    }

    void OnDrawGizmos()
    {
            // show blocked squares
            if(showObstacles){
                Gizmos.color = new Color(1, 0.5f, 0.5f, 0.5f);
                foreach(KeyValuePair<Tuple<int,int>,int> pair in obstacles){
                    if(pair.Value == Int32.MaxValue){
                        Gizmos.DrawCube(cg.tupleToWorld(pair.Key) + (Vector3.forward * cellSize / 2) + (Vector3.right * cellSize / 2), new Vector3(cellSize,cellSize,cellSize));
                    }
                }
            }


            // TODO: SAVE DIJKSTRA GRID IN A STATIC CLASS TO ACCESS FOR DEBUG DRAWING
            // show dijkstra values
            // if(showDijkstra){
            //     for (int i = 0; i < dGrid.GetLength(0); i++){
            //         for(int j = 0; j < dGrid.GetLength(1); j++){
            //             string val = dGrid[i,j].ToString();
            //             UnityEditor.Handles.Label(new Vector3((i - 10) * 5, 1.0f, (j - 20) * 5), val);
            //         }
            //     }
            // }

            // show vectors
            if(showVectors){
                Gizmos.color = new Color(1, 1.0f, 1.0f, 1.0f);
                foreach(KeyValuePair<Tuple<int,int>,Vector3> pair in ff){
                    DrawArrow.ForDebug(cg.tupleToWorld(pair.Key) + (Vector3.forward * cellSize / 2) + (Vector3.right * cellSize / 2),pair.Value);
                }
            }
    }
}
