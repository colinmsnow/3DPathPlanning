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


    Hovering around 1000 ms for .5 grid and 36 ms for 1 grid and 29045 for .25 grid


    Notes:

    Use priority queue to choose elements (heap)




*/

public class Astar
{

    public MapGrid grid;
    public float[] target;
    public float[] origin;

    HashSet<Cell> openList = new HashSet<Cell>();
    HashSet<Cell> closedList = new HashSet<Cell>();
    // C5.IntervalHeap<Cell> heap = new IntervalHeap<Cell>(1000, new CellCompare());
    PriorityQueue<Cell> queue = new PriorityQueue<Cell>(true);

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

        int numiters = 0;




        originCell.heuristic = grid.euclideanDistanceCell(originCell, targetCell);
        openList.Add(originCell);
        queue.Enqueue(0, originCell);
        // heap.Add(originCell);
        remaining++;

        while (remaining>0){ // It isnt empty (check if this works)

            // Debug.Log(originCell);
            // openList.Remove(originCell);
            // remaining--;

            numiters++;


            // Cell bestCell = GetBestCell(); // Dont do this
            Cell bestCell = queue.Dequeue();
            openList.Remove(bestCell);
            remaining--;

            var neighbours = grid.GetNeighbours(bestCell);

            // for(int i=0;i<27;i++){
            //     Debug.Log(neighbours[i].empty);
            // }


            // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // cube.transform.localScale = new Vector3(grid.boxSize/4, grid.boxSize/4, grid.boxSize/4);
            // cube.transform.position = CellToVector(bestCell);
            // Color theColorToAdjust = Color.yellow;
            // theColorToAdjust.a = 0f; // Completely transparent
            // cube.GetComponent<Renderer>().material.color = theColorToAdjust;


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
                    queue.Enqueue(curCell.f, curCell);
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

    public Vector3 CellToVector(Cell cell){
    
        Vector3 startpos = new Vector3(grid.startPosition[0], grid.startPosition[1], grid.startPosition[2]);
        Vector3 boxpos = new Vector3(cell.position[0], cell.position[1], cell.position[2]);
        boxpos *=grid.boxSize;
        boxpos = boxpos + startpos;
        return boxpos;
    }

    





}



public class CellCompare : Comparer<Cell>{
    public override int Compare(Cell a, Cell b){

        if(a.f > b.f) return 1;
        return -1;
    }
}



public class PriorityQueue<T>
{
    class Node
    {
        public float Priority { get; set; }
        public T Object { get; set; }
    }

    //object array
    List<Node> queue = new List<Node>();
    int heapSize = -1;
    bool _isMinPriorityQueue;
    public int Count { get { return queue.Count; } }

    /// <summary>
    /// If min queue or max queue
    /// </summary>
    /// <param name="isMinPriorityQueue"></param>
    public PriorityQueue(bool isMinPriorityQueue = false)
    {
        _isMinPriorityQueue = isMinPriorityQueue;
    }


    /// <summary>
    /// Enqueue the object with priority
    /// </summary>
    /// <param name="priority"></param>
    /// <param name="obj"></param>
    public void Enqueue(float priority, T obj)
    {
        Node node = new Node() { Priority = priority, Object = obj };
        queue.Add(node);
        heapSize++;
        //Maintaining heap
        if (_isMinPriorityQueue)
            BuildHeapMin(heapSize);
        else
            BuildHeapMax(heapSize);
    }
        /// <summary>
    /// Dequeue the object
    /// </summary>
    /// <returns></returns>
    public T Dequeue()
    {
        if (heapSize > -1)
        {
            var returnVal = queue[0].Object;
            queue[0] = queue[heapSize];
            queue.RemoveAt(heapSize);
            heapSize--;
            //Maintaining lowest or highest at root based on min or max queue
            if (_isMinPriorityQueue)
                MinHeapify(0);
            else
                MaxHeapify(0);
            return returnVal;
        }
        else
            throw new Exception("Queue is empty");
    }
    /// <summary>
/// Updating the priority of specific object
/// </summary>
/// <param name="obj"></param>
/// <param name="priority"></param>
public void UpdatePriority(T obj, int priority)
{
    int i = 0;
    for (; i <= heapSize; i++)
    {
        Node node = queue[i];
        if (object.ReferenceEquals(node.Object, obj))
        {
            node.Priority = priority;
            if (_isMinPriorityQueue)
            {
                BuildHeapMin(i);
                MinHeapify(i);
            }
            else
            {
                BuildHeapMax(i);
                MaxHeapify(i);
            }
        }
    }
}
/// <summary>
/// Searching an object
/// </summary>
/// <param name="obj"></param>
/// <returns></returns>
public bool IsInQueue(T obj)
{
    foreach (Node node in queue)
        if (object.ReferenceEquals(node.Object, obj))
            return true;
    return false;
}

    /// <summary>
    /// Maintain max heap
    /// </summary>
    /// <param name="i"></param>
    private void BuildHeapMax(int i)
    {
        while (i >= 0 && queue[(i - 1) / 2].Priority < queue[i].Priority)
        {
            Swap(i, (i - 1) / 2);
            i = (i - 1) / 2;
        }
    }
    /// <summary>
    /// Maintain min heap
    /// </summary>
    /// <param name="i"></param>
    private void BuildHeapMin(int i)
    {
        while (i >= 0 && queue[(i - 1) / 2].Priority > queue[i].Priority)
        {
            Swap(i, (i - 1) / 2);
            i = (i - 1) / 2;
        }
    }
        private void MaxHeapify(int i)
    {
        int left = ChildL(i);
        int right = ChildR(i);

        int heighst = i;

        if (left <= heapSize && queue[heighst].Priority < queue[left].Priority)
            heighst = left;
        if (right <= heapSize && queue[heighst].Priority < queue[right].Priority)
            heighst = right;

        if (heighst != i)
        {
            Swap(heighst, i);
            MaxHeapify(heighst);
        }
    }
    private void MinHeapify(int i)
    {
        int left = ChildL(i);
        int right = ChildR(i);

        int lowest = i;

        if (left <= heapSize && queue[lowest].Priority > queue[left].Priority)
            lowest = left;
        if (right <= heapSize && queue[lowest].Priority > queue[right].Priority)
            lowest = right;

        if (lowest != i)
        {
            Swap(lowest, i);
            MinHeapify(lowest);
        }
    }

        private void Swap(int i, int j)
        {
            var temp = queue[i];
            queue[i] = queue[j];
            queue[j] = temp;
        }
        private int ChildL(int i)
        {
            return i * 2 + 1;
        }
        private int ChildR(int i)
        {
            return i * 2 + 2;
        }
}