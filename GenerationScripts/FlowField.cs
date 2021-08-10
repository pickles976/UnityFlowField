using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// This class takes in a Dijkstra grid and returns a
/// 2D array of Vector3's representing the FlowField
public class FlowField
{
    public static Vector3[,] Generate(int numRows,int numCols,int[,] dGrid,Tuple<int,int> b1,CustomGrid cg)
    {

        float cellSize = cg.cellSize;

        int ROW = numRows;
        int COL = numCols;

        int rOff = b1.Item1;
        int cOff = b1.Item2;

        int [,] dijkstra = dGrid;
        Vector3[,] flowfield = new Vector3[ROW, COL];

        for (int j = 0; j < COL; j++)
        {
            for(int i = 0; i < ROW; i++)
            {
                int min = Int32.MaxValue;
                int j_dest = -1; //the indices of the cell with the smallest cost value
                int i_dest = -1; //set these to -1 so we know if they are not set

                //(i,j+1) EAST
                if (j + 1 < COL)
                {
                    if ((dijkstra[i,j+1] < min) && (dijkstra[i, j + 1] != -1))
                    {
                        min = dijkstra[i, j + 1];
                        i_dest = i;
                        j_dest = j + 1;
                    }
                }

                //(i-1,j+1) SOUTH-EAST
                if ((j + 1 < COL) && (i > 0))
                {
                    if ((dijkstra[i - 1, j + 1] < min) && (dijkstra[i - 1, j + 1] != -1))
                    {
                        min = dijkstra[i - 1, j + 1];
                        i_dest = i - 1;
                        j_dest = j + 1;
                    }
                }

                //(i,j+1) SOUTH
                if (i > 0)
                {
                    if ((dijkstra[i - 1, j] < min) && (dijkstra[i - 1, j] != -1))
                    {
                        min = dijkstra[i - 1, j];
                        i_dest = i - 1;
                        j_dest = j;
                    }
                }

                //(i-1,j-1) SOUTH-WEST
                if ((j > 0) && (i > 0))
                {
                    if ((dijkstra[i - 1, j - 1] < min) && (dijkstra[i - 1, j - 1] != -1))
                    {
                        min = dijkstra[i - 1, j - 1];
                        i_dest = i - 1;
                        j_dest = j - 1;
                    }
                }

                //(i,j+1) WEST
                if (j > 0 )
                {
                    if ((dijkstra[i, j - 1] < min) && (dijkstra[i, j - 1] != -1))
                    {
                        min = dijkstra[i, j - 1];
                        i_dest = i;
                        j_dest = j - 1;
                    }
                }

                //(i+1,j-1) NORTH-WEST
                if ((j > 0) && (i + 1 < ROW))
                {
                    if ((dijkstra[i + 1, j - 1] < min) && (dijkstra[i + 1, j - 1] != -1))
                    {
                        min = dijkstra[i + 1, j - 1];
                        i_dest = i + 1;
                        j_dest = j - 1;
                    }
                }

                //(i,j+1) NORTH
                if (i + 1 < ROW)
                {
                    if ((dijkstra[i + 1, j] < min) && (dijkstra[i + 1, j] != -1))
                    {
                        min = dijkstra[i + 1, j];
                        i_dest = i + 1;
                        j_dest = j;
                    }
                }

                //(i+1,j-1) NORTH-EAST
                if ((j + 1 < COL) && (i + 1 < ROW))
                {
                    if ((dijkstra[i + 1, j + 1] < min) && (dijkstra[i + 1, j + 1] != -1))
                    {
                        min = dijkstra[i + 1, j + 1];
                        i_dest = i + 1;
                        j_dest = j + 1;
                    }
                }

                Vector3 field = new Vector3();

                field.y = 0.0f;
                field.x = (float)(i_dest - i);
                field.z = (float)(j_dest - j);

                if((i_dest == -1) || (j_dest == -1))
                {
                    field = new Vector3(0, 0, 0);
                }

                Color grad = new Vector4(0.01f * dijkstra[i, j], 0.0f,0.0f, 1);

                flowfield[i, j] = field/(field.magnitude); //normalize vector

            }//end for j
        }//end for i

        return flowfield;

    }//end function

    


}