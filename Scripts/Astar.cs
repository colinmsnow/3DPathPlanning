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

    Might be an issue with the score not taking into account the previous path
    and instead just taking straight line distance?? not sure.

    Probably best to just rewrite the algorithm so I know its right. Grrr.

    Should definitely be a min priority queue so something is definitely messed up.

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

        // openList.Clear();
        // closedList.Clear();

        Cell originCell = grid.findCellByXYZ(origin[0], origin[1], origin[2]);
        Cell targetCell = grid.findCellByXYZ(target[0], target[1], target[2]);


        GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        cube1.transform.localScale = new Vector3(2f, 2f, 2f);
        cube1.transform.position = CellToVector(originCell);
        // Color theColorToAdjust = Color.yellow;
        // theColorToAdjust.a = 0f; // Completely transparent
        cube1.GetComponent<Renderer>().material.color = Color.blue;

        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        cube2.transform.localScale = new Vector3(2f, 2f, 2f);
        cube2.transform.position = CellToVector(targetCell);
        // Color theColorToAdjust = Color.yellow;
        // theColorToAdjust.a = 0f; // Completely transparent
        cube2.GetComponent<Renderer>().material.color = Color.cyan;

        originCell.cost = 0;
        originCell.heuristic = grid.Distance(originCell, targetCell);
        queue.Enqueue(originCell.f, originCell);

        // Debug.Log("Got here");

        // Debug.Log(string.Format("Queue is: {0}", queue.isEmpty()));

        while(!queue.isEmpty()){

            // Debug.Log("looped");
            
            
            Cell current = queue.Dequeue();
            if (current == targetCell){
                return CreatePath(current);
            }

            var neighbours = grid.GetNeighbours(current);

            // Debug.Log(neighbours.Length);


            for (int i = 0; i < neighbours.Length; i++){



                Cell neighbour = neighbours[i];


                GameObject checkedneighbour = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                checkedneighbour.transform.localScale = new Vector3(1f, 1f, 1f);
                checkedneighbour.transform.position = CellToVector(neighbour);
                checkedneighbour.GetComponent<Renderer>().material.color = Color.yellow;

                // Debug.Log(neighbour);
                // Debug.Log(neighbour.empty);

                if(neighbour == null){
                    continue;
                }


                if (!neighbour.empty){
                    continue;
                }


                // Debug.Log("Current cost is: " + current.cost.ToString());
                // Debug.Log("Grid distance is: " + grid.Distance(current, neighbour).ToString());



                float tentativeScore = current.cost + grid.Distance(current, neighbour);


                // Debug.Log("Checking tentative score");

                if (tentativeScore < neighbour.cost){

                    // Debug.Log("tentative score is less");

                    neighbour.parent = current;
                    neighbour.cost = tentativeScore;
                    neighbour.heuristic = grid.Distance(targetCell, neighbour);

                    if (!queue.IsInQueue(neighbour)){
                        queue.Enqueue(neighbour.f, neighbour);
                    }
                }
            }
        }
        return null;
    }

    private List<Cell> CreatePath(Cell destination){

        var path = new List<Cell>() { destination };

        var current = destination;

        if (current.floatPosition != null){
        while (current.parent != null)
            {

                var neighbours = current.neighbours;

                // foreach (Cell neighbour in neighbours){
                //     Debug.DrawRay(floattovec3(current.floatPosition), floattovec3(neighbour.floatPosition) - floattovec3(current.floatPosition), Color.red, 1000, false);
                // }
                // Debug.DrawRay(floattovec3(current.floatPosition), floattovec3(parent.floatPosition) - floattovec3(current.floatPosition), Color.red, 1000, false);
                current = current.parent;
                path.Add(current);

                
            }
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

    public float StraightLineRatio(){
        List<Vector3> vectorPath = grid.PathToVectors(generatedPath);
        float optimal = Vector3.Distance(vectorPath[0], vectorPath[vectorPath.Count-1]);
        float sum = 0;
        for(int i=1; i<vectorPath.Count; i++){
            sum += Vector3.Distance(vectorPath[i], vectorPath[i-1]);
        }
        return sum/optimal;
    }

    private float[] vec3tofloat(Vector3 input){
        return new float[] {input[0], input[1], input[2]};
        
    }

    private Vector3 floattovec3(float[] input){
        return new Vector3 (input[0], input[1], input[2]);
        
    }
}