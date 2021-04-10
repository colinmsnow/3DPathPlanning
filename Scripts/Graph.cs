using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/*
    Abstract class to define a graph object for use with A*
    Declares all functions required to calculate a path
*/

[Serializable]
public abstract class Graph
{
    public abstract float Distance(Cell a, Cell b);
    public abstract Cell findCellByXYZ(float x, float y, float z);
    public abstract Cell[] GetNeighbours(Cell cell);
    public abstract List<Vector3> PathToVectors(List<Cell> generatedPath);
    public abstract Vector3 CellToVector(Cell cell);
    public abstract Cell[,,] createGraph();
    public abstract void setMaskItem(int x, int y, int z, bool value);
}