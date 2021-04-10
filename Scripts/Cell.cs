using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/*

    A node of a graph.

    Has many fields which can be used by different algorithms
    to define its spot in a graph, in the world, and relative
    to other nodes as well as costs and heiristic values.

*/


[Serializable]
public class Cell {

    public int[] position = new int[] {0, 0, 0};
    public float[] floatPosition;
    public bool empty;

    public float cost, heuristic;
    public float f
    {
        get { return cost + heuristic; }
    }

    public Cell parent = null;

    public List<Cell> neighbours;

    private Cell() { }

    public Cell(bool _empty, int _x, int _y, int _z){

        empty = _empty;
        position[0] = _x;
        position[1] = _y;
        position[2] = _z;

    }

    public void printCell(){
        Debug.Log(string.Format("Cell position: {0}, {1}, {2}, empty: {3}, parent: {4}, cost: {5}, heuristic: {6}", position[0], position[1], position[2], empty, parent, cost, heuristic));
    }

}