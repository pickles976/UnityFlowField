using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Class for generating Dijkstra Grids
public class DijkstraGrid
{

    //Row and Column depend on grid size
    public static int COL;
    public static int ROW;

    struct cell
    {
        //Row and Column index of its parent
        //Note that 0 <= i <= ROW - 1 & 0 <= j <= COL - 1
        public int parent_i, parent_j; //indices of parent cell
        public int row, col; //indices in cell array
        public int distance; //distance from the source
    }

    //Utility function to check whether a given cell is valid or not
    static bool isValid(int row, int col)
    {
        //returns true if row# and col# is within range
        return (row >= 0) && (row < ROW) &&
               (col >= 0) && (col < COL);
    }

    //utility function to check whether the given cell is blocked or not
    static bool isUnBlocked(int[,] grid, int row, int col)
    {
        //returns true if the cell is not clocked else false
        if (grid[row, col] == Int32.MaxValue)
        {
            return (false);
        }
        else
        {
            return (true);
        }
    }

    //Utility function to check whether destination cell has been reached or not
    static bool isDestination(int row, int col, Tuple<int, int> dest)
    {
        if (row == dest.Item1 && col == dest.Item2)
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }

    //generate djikstra grid on a given obstacle grid
    public static int[,] GenerateGrid(int numCols,int numRows,Tuple<int,int> b1, Dictionary<Tuple<int,int>,int> obstacleGrid, Tuple<int, int> destination)
    {
        int rOff = b1.Item2;
        int cOff = b1.Item1;

        Tuple<int,int> dest = new Tuple<int,int>(destination.Item1 - rOff, destination.Item2 - cOff);

        ROW = numRows;
        COL = numCols;

        int i;
        int j;

        int[,] dijkstraGrid = new int[ROW,COL]; //give the djikstra grid blocked spaces

        foreach(KeyValuePair<Tuple<int,int>,int> pair in obstacleGrid){
            int r = pair.Key.Item1;
            int c = pair.Key.Item2;
            if (r - rOff < ROW && c - cOff < COL){
                dijkstraGrid[r - rOff,c - cOff] = pair.Value;
            }
        }

        //check if the destination is out of range
        if (isValid(dest.Item1,dest.Item2) == false)
        {
            Debug.Log("Dijkstra destination is invalid");
            return null;
        }

        //Destination is blocked
        if (isUnBlocked(dijkstraGrid, dest.Item1,dest.Item2) == false)
        {
            Debug.Log("Dijkstra destination is blocked");
            Debug.Log("Finding a new destination...");

            dest = FindNearestUnblocked(dijkstraGrid,dest,COL,ROW);

            if(isValid(dest.Item1,dest.Item2) && isUnBlocked(dijkstraGrid, dest.Item1,dest.Item2)){
                Debug.Log("New destination found!");
            }else{
                Debug.Log("No new destination could be found");
                return null;
            }
        }

        //FLOOD FILL FROM THE END POINT

        //declare a 2D array of cell structures to hold the details of that cell
        var cellDetails = new cell[ROW, COL];

        //initialize with default values for cellDetails
        for (i = 0; i < ROW; i++)
        {
            for (j = 0; j < COL; j++) { 
                cellDetails[i, j].parent_i = -1;
                cellDetails[i, j].parent_j = -1;
                cellDetails[i, j].distance = -1;
                cellDetails[i, j].row = i;
                cellDetails[i, j].col = j;
            }
        }

        //initialize destination cell
        cellDetails[dest.Item1, dest.Item2].distance = 0;

        Queue<cell> toVisit = new Queue<cell>(); //FIFO STRUCTURE

        i = dest.Item1; //row
        j = dest.Item2; //col

        //SET PARENTS AND ADD Item1 FOUR NEIGHBOURS TO QUEUE
        //WORKING
        if (j + 1 < COL) //check for within bounds
        {

                cellDetails[dest.Item1, dest.Item2 + 1].parent_i = i;
                cellDetails[dest.Item1, dest.Item2 + 1].parent_j = j;
                toVisit.Enqueue(cellDetails[dest.Item1, dest.Item2 + 1]); //right

        }

        if (i > 0)
        {

                cellDetails[dest.Item1 - 1, dest.Item2].parent_i = i;
                cellDetails[dest.Item1 - 1, dest.Item2].parent_j = j;
                toVisit.Enqueue(cellDetails[dest.Item1 - 1, dest.Item2]); //down\

        }

        if (j > 0)
        {

                cellDetails[dest.Item1, dest.Item2 - 1].parent_i = i;
                cellDetails[dest.Item1, dest.Item2 - 1].parent_j = j;
                toVisit.Enqueue(cellDetails[dest.Item1, dest.Item2 - 1]); //left

        }

        if (i + 1 < ROW)
        {

                cellDetails[dest.Item1 + 1, dest.Item2].parent_i = i;
                cellDetails[dest.Item1 + 1, dest.Item2].parent_j = j;
                toVisit.Enqueue(cellDetails[dest.Item1 + 1, dest.Item2]); //up

        }

        while (toVisit.Count > 0)
        {
            cell tempCell = toVisit.Dequeue(); //pull cell of the QUEUE

            //check if the destination is out of range
            if (isValid(tempCell.row, tempCell.col))
            {
                    if (cellDetails[tempCell.row, tempCell.col].distance == -1)
                    {

                    //Check if destination is blocked
                    if (isUnBlocked(dijkstraGrid,tempCell.row, tempCell.col))
                    {
                            //grab the parent's indices
                            int parent_i = cellDetails[tempCell.row, tempCell.col].parent_i;
                            int parent_j = cellDetails[tempCell.row, tempCell.col].parent_j;

                            //calculate distance from parents
                            cellDetails[tempCell.row, tempCell.col].distance = cellDetails[parent_i,parent_j].distance + 1;

                            //change to the indices of the currently selected cell
                            i = tempCell.row;
                            j = tempCell.col;


                        if (j + 1 < COL) //check for within bounds
                        {
                            //check if it has been unvisited (parent set to -1,-1)
                            if ((cellDetails[i, j + 1].parent_i == -1) && (cellDetails[i, j + 1].parent_j == -1)){
                                cellDetails[i, j + 1].parent_i = i;
                                cellDetails[i, j + 1].parent_j = j;
                                toVisit.Enqueue(cellDetails[i, j + 1]); //right
                            }
                        }

                        if (i > 0)
                        {
                            if ((cellDetails[i - 1, j].parent_i == -1) && (cellDetails[i - 1, j].parent_j == -1))
                            {
                                cellDetails[i - 1, j].parent_i = i;
                                cellDetails[i - 1, j].parent_j = j;
                                toVisit.Enqueue(cellDetails[i - 1, j]); //down
                            }
                        }

                        if (j > 0)
                        {
                            if ((cellDetails[i, j - 1].parent_i == -1) && (cellDetails[i, j - 1].parent_j == -1))
                            {
                                cellDetails[i, j - 1].parent_i = i;
                                cellDetails[i, j - 1].parent_j = j;
                                toVisit.Enqueue(cellDetails[i, j - 1]); //left
                            }
                        }

                        if (i + 1 < ROW)
                        {
                            if ((cellDetails[i + 1, j].parent_i == -1) && (cellDetails[i + 1, j].parent_j == -1))
                            {
                                cellDetails[i + 1, j].parent_i = i;
                                cellDetails[i + 1, j].parent_j = j;
                                toVisit.Enqueue(cellDetails[i + 1, j]); //up
                            }
                        }

                        }
                        else
                        {
                            //calculate distance from parents
                            cellDetails[tempCell.row, tempCell.col].distance = Int32.MaxValue;
                        }
                    }//end if uninitialized
            }//end if isValid
        }//end while

        //transfer distance values to the Dijkstra grid
        for(i = 0; i < ROW; i++)
        {
            //String s = "";
            for(j = 0; j < COL; j++)
            {
                    if (dijkstraGrid[i, j] != Int32.MaxValue)
                    {
                        dijkstraGrid[i, j] = cellDetails[i, j].distance;
                    }
            }
        }

        return dijkstraGrid;

    }

    //PERFORM BREADTH-FIRST SEARCH TO FIND NEAREST UNBLOCKED CELL
    public static Tuple<int,int> FindNearestUnblocked(int[,] obstacleGrid, Tuple<int, int> dest,int rowLength,int colLength)
    {
        int dest_i = dest.Item1;
        int dest_j = dest.Item2;

        if (obstacleGrid[dest_i,dest_j] == Int32.MaxValue)
        {
            //declare a 2D array of cell structures to hold the details of that cell
            var cellDetails = new cell[rowLength, colLength];
            int i;
            int j;

            //initialize with default values for cellDetails
            for (i = 0; i < rowLength; i++)
            {
                for (j = 0; j < colLength; j++)
                {
                    cellDetails[i, j].parent_i = -1;
                    cellDetails[i, j].parent_j = -1;
                    cellDetails[i, j].row = i;
                    cellDetails[i, j].col = j;
                    cellDetails[i, j].distance = -1;
                }
            }

            i = 0;
            j = 0;

            Queue<cell> toVisit = new Queue<cell>(); //FIFO STRUCTURE

            //SET PARENTS AND ADD FIRST FOUR NEIGHBOURS TO QUEUE
            //WORKING
            if (dest_j + 1 < rowLength) //check for within bounds
            {
                cellDetails[dest.Item1, dest.Item2 + 1].parent_i = dest_i;
                cellDetails[dest.Item1, dest.Item2 + 1].parent_j = dest_j;
                toVisit.Enqueue(cellDetails[dest.Item1, dest.Item2 + 1]); //right
            }

            if (dest_i > 0)
            {
                cellDetails[dest.Item1 - 1, dest.Item2].parent_i = dest_i;
                cellDetails[dest.Item1 - 1, dest.Item2].parent_j = dest_j;
                toVisit.Enqueue(cellDetails[dest.Item1 - 1, dest.Item2]); //down
            }

            if (dest_j > 0)
            {
                cellDetails[dest.Item1, dest.Item2 - 1].parent_i = dest_i;
                cellDetails[dest.Item1, dest.Item2 - 1].parent_j = dest_j;
                toVisit.Enqueue(cellDetails[dest.Item1, dest.Item2 - 1]); //left
            }

            if (dest_i + 1 < colLength)
            {
                cellDetails[dest.Item1 + 1, dest.Item2].parent_i = dest_i;
                cellDetails[dest.Item1 + 1, dest.Item2].parent_j = dest_j;
                toVisit.Enqueue(cellDetails[dest.Item1 + 1, dest.Item2]); //up
            }

            while (toVisit.Count > 0)
            {
                cell tempCell = toVisit.Dequeue(); //pull cell of the QUEUE

                    if (cellDetails[tempCell.row, tempCell.col].distance == -1)
                    {

                        //Destination is blocked
                        if (obstacleGrid[tempCell.row,tempCell.col] == int.MaxValue)
                        {

                            //change to the indices of the currently selected cell
                            i = tempCell.row;
                            j = tempCell.col;


                            if (j + 1 < colLength) //check for within bounds
                            {
                                //check if it has been unvisited (parent set to -1,-1)
                                if ((cellDetails[i, j + 1].parent_i == -1) && (cellDetails[i, j + 1].parent_j == -1))
                                {
                                    cellDetails[i, j + 1].parent_i = i;
                                    cellDetails[i, j + 1].parent_j = j;
                                    toVisit.Enqueue(cellDetails[i, j + 1]); //right
                                }
                            }

                            if (i > 0)
                            {
                                if ((cellDetails[i - 1, j].parent_i == -1) && (cellDetails[i - 1, j].parent_j == -1))
                                {
                                    cellDetails[i - 1, j].parent_i = i;
                                    cellDetails[i - 1, j].parent_j = j;
                                    toVisit.Enqueue(cellDetails[i - 1, j]); //down
                                }
                            }

                            if (j > 0)
                            {
                                if ((cellDetails[i, j - 1].parent_i == -1) && (cellDetails[i, j - 1].parent_j == -1))
                                {
                                    cellDetails[i, j - 1].parent_i = i;
                                    cellDetails[i, j - 1].parent_j = j;
                                    toVisit.Enqueue(cellDetails[i, j - 1]); //left
                                }
                            }

                            if (i + 1 < rowLength)
                            {
                                if ((cellDetails[i + 1, j].parent_i == -1) && (cellDetails[i + 1, j].parent_j == -1))
                                {
                                    cellDetails[i + 1, j].parent_i = i;
                                    cellDetails[i + 1, j].parent_j = j;
                                    toVisit.Enqueue(cellDetails[i + 1, j]); //up
                                }
                            }

                        }
                        else
                        {
                            //calculate distance from parents
                            return new Tuple<int, int>(tempCell.row,tempCell.col);
                        }
                    }//end if uninitialized
            }//end while
        }

        return new Tuple<int, int>(dest_i,dest_j);
    }
}