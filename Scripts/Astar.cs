using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* Implements the A* algorithm in 3d.

Inputs:
    a Graph Object
    An origin point
    A target point

Returns:

    An array of 3D points that form a path from 
        origin to target


    Hovering around 1000 ms for .5 grid and 36 ms for 1 grid and 29045 for .25 grid
    Heap around 479 ms for .5 grid and 35 ms for 1 grid and 16380 for .25 grid

*/

public class Astar
{

    public Graph grid;
    public float[] target;
    public float[] origin;

    HashSet<Cell> openList = new HashSet<Cell>();
    HashSet<Cell> closedList = new HashSet<Cell>();
    PriorityQueue<Cell> queue = new PriorityQueue<Cell>(true);

    private List<Cell> generatedPath;


    private Astar () { }

    public Astar (Graph _grid){
        grid = _grid;
    }

    public List<Cell> search(float[] _origin, float[] _target){

        target = _target;
        origin = _origin;

        openList.Clear();
        closedList.Clear();

        Cell originCell = grid.findCellByXYZ(origin[0], origin[1], origin[2]);
        Cell targetCell = grid.findCellByXYZ(target[0], target[1], target[2]);

        int remaining = 0;
        int numiters = 0;


        originCell.heuristic = grid.Distance(originCell, targetCell);
        openList.Add(originCell);
        queue.Enqueue(0, originCell);
        remaining++;

        while (remaining>0){ // It isnt empty

            numiters++;

            Cell bestCell = queue.Dequeue();
            openList.Remove(bestCell);
            remaining--;

            var neighbours = grid.GetNeighbours(bestCell);


            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localScale = new Vector3(.25f, .25f, .25f);
            cube.transform.position = CellToVector(bestCell);
            Color theColorToAdjust = Color.yellow;
            theColorToAdjust.a = 0f; // Completely transparent
            cube.GetComponent<Renderer>().material.color = theColorToAdjust;


            for (int i = 0; i < 26; i++)
            {
                var curCell = neighbours[i];

                if (curCell == null)
                    continue;
                if (curCell == targetCell)
                {
                    curCell.parent = bestCell;
                    Debug.Log(string.Format("Num iterations: {0}", numiters));
                    return CreatePath(curCell);
                }

                if (!curCell.empty){
                    continue;
                }

                var g = bestCell.cost + grid.Distance(curCell, bestCell);

                var h = grid.Distance(curCell, targetCell);

                if (openList.Contains(curCell) && curCell.f < (g + h))
                    continue;
                if (closedList.Contains(curCell) && curCell.f < (g + h))
                    continue;

                curCell.cost = g;
                curCell.heuristic = h;
                curCell.parent = bestCell;

                if (!closedList.Contains(bestCell))
                    closedList.Add(bestCell);

                if (!openList.Contains(curCell)){
                    openList.Add(curCell);
                    queue.Enqueue(curCell.f, curCell);
                    remaining++;
                }
            }
        
        }
        return null;
    }


    private List<Cell> CreatePath(Cell destination){

        var path = new List<Cell>() { destination };

        var current = destination;
        while (current.parent != null)
        {
            current = current.parent;
            path.Add(current);
        }

        path.Reverse();
        generatedPath = path;

        return generatedPath;
    }

    public List<Vector3> PathToVectors(){
        List<Vector3> vectorPath = grid.PathToVectors(generatedPath);
        return vectorPath;
    }

    public Vector3 CellToVector(Cell cell){
        return grid.CellToVector(cell);
    }
}