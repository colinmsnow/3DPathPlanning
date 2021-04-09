using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* Implements the A* algorithm in 3d.

Inputs:
    a MapGrid Object
    An origin point
    A target point

Returns:

    An array of 3D points that form a path from 
        origin to target

*/

public class Astar
{

    public MapGrid grid;
    public float[] target;
    public float[] origin;

    HashSet<Cell> openList = new HashSet<Cell>();
    HashSet<Cell> closedList = new HashSet<Cell>();

    private List<Cell> generatedPath;


    private Astar () { }

    public Astar (MapGrid _grid){
        grid = _grid;
        // target = _target;
        // origin = _origin;

        // HashSet<Cell> openList = new HashSet<Cell>();
        // HashSet<Cell> closedList = new HashSet<Cell>();
    }

    public List<Cell> search(float[] _origin, float[] _target){

        // Hash set can do Add, Contains, Clear, GetEnumerator, Remove, ToString, etc...


        target = _target;
        origin = _origin;

        openList.Clear();
        closedList.Clear();

        Cell originCell = grid.findCellByXYZ(origin[0], origin[1], origin[2]);
        Cell targetCell = grid.findCellByXYZ(target[0], target[1], target[2]);

        int remaining = 0;

        Debug.Log(originCell.position[0]);
        Debug.Log(originCell.position[1]);
        Debug.Log(originCell.position[2]);

        Debug.Log(targetCell.position[0]);
        Debug.Log(targetCell.position[1]);
        Debug.Log(targetCell.position[2]);




        originCell.heuristic = grid.euclideanDistanceCell(originCell, targetCell);
        openList.Add(originCell);
        remaining++;

        while (remaining>0){ // It isnt empty (check if this works)

            // Debug.Log(originCell);
            // openList.Remove(originCell);
            // remaining--;


            Cell bestCell = GetBestCell();
            openList.Remove(bestCell);
            remaining--;

            var neighbours = grid.GetNeighbours(bestCell);

            // for(int i=0;i<27;i++){
            //     Debug.Log(neighbours[i].empty);
            // }



            for (int i = 0; i < 26; i++)
            {
                var curCell = neighbours[i];

                if (curCell == null)
                    continue;
                if (curCell == targetCell)
                {
                    curCell.parent = bestCell;
                    return CreatePath(curCell);
                }

                if (!curCell.empty){
                    continue;
                }

                // var g = bestCell.cost + (curCell.position - bestCell.position).magnitude;
                var g = bestCell.cost + grid.euclideanDistanceCell(curCell, bestCell);

                var h = grid.euclideanDistanceCell(curCell, targetCell);

                if (openList.Contains(curCell) && curCell.f < (g + h))
                    continue;
                if (closedList.Contains(curCell) && curCell.f < (g + h))
                    continue;

                curCell.cost = g;
                curCell.heuristic = h;
                curCell.parent = bestCell;

                if (!closedList.Contains(bestCell))
                closedList.Add(bestCell);

                if (!openList.Contains(curCell))
                    openList.Add(curCell);
                    remaining++;
            }
        
        }

        return null;


    }

    private Cell GetBestCell ()
    {
        Cell result = null;
        float currentF = float.PositiveInfinity;

        foreach(Cell cell in openList)
        {
            if (cell.f < currentF)
            {
                currentF = cell.f;
                result = cell;
            }
        }

        return result;
    }

    private List<Cell> CreatePath(Cell destination){

        var path = new List<Cell>() { destination };

        var current = destination;
        while (current.parent != null)
        // for (int i=0; i<4; i++)
        {
            current = current.parent;
            path.Add(current);
            current.printCell();
        }

        path.Reverse();
        generatedPath = path;

        return generatedPath;
    }

    public List<Vector3> PathToVectors(){

        List<Vector3> vectorPath = new List<Vector3>();
        Vector3 startpos = new Vector3(grid.startPosition[0], grid.startPosition[1], grid.startPosition[2]);

        foreach(Cell cell in generatedPath){
            Vector3 boxpos = new Vector3(cell.position[0], cell.position[1], cell.position[2]);
            boxpos *=grid.boxSize;
            boxpos = boxpos + startpos;
            // boxpos *=grid.boxSize;
            vectorPath.Add(boxpos);
        }



        return vectorPath;
    }

    





}