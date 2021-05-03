using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
// using System.Runtime.InteropServices;


/* 

Stores the grid for a scene and associated methods


*/
[Serializable]
public class PointGraph: Graph{


    public PointGraph(){
    }

    private List<Vector3> points;
    private List<Cell> cells;
    private bool[,] connectedGrid;


    public void initialize(List<Vector3> _points, bool[,] _connectedGrid){
        points = _points;
        connectedGrid = _connectedGrid;
        cells = new List<Cell>();
        
    }

    public override void setMaskItem(int x, int y, int z, bool value){
    }

    /* Gets euclidean distance between two cells */
    public override float Distance(Cell one, Cell two){
        float[] a = one.floatPosition;
        float[] b = two.floatPosition;
        // return (1 / FastInvSqrt((a[0]-b[0])*(a[0]-b[0]) + (a[1]-b[1])*(a[1]-b[1]) + (a[2]-b[2])*(a[2]-b[2]))) * boxSize;
        return Mathf.Sqrt((a[0]-b[0])*(a[0]-b[0]) + (a[1]-b[1])*(a[1]-b[1]) + (a[2]-b[2])*(a[2]-b[2]));
    }

    public override Cell[,,] createGraph(){
        Cell[,,] _cellGrid = new Cell[points.Count, 1, 1];



        for(int i=0; i<points.Count; i++){
            Cell newCell = new Cell(false, 0, 0, 0);
            newCell.floatPosition = vec3tofloat(points[i]);
            newCell.empty = true;
            newCell.neighbours = new List<Cell>();
            cells.Add(newCell);
        }

        for(int i=0; i<points.Count; i++){
            // Cell newCell = new Cell(false, 0, 0, 0);
            // newCell.floatPosition = vec3tofloat(points[i]);
            // cells.Add(newCell);
            for(int j=0; j<points.Count; j++){
                if(connectedGrid[i, j]){
                    cells[i].neighbours.Add(cells[j]);
                }
            }
        }



        return _cellGrid;
    }


    public override Cell findCellByXYZ(float x, float y, float z){
        
        // x = x - startPosition[0];
        // y = y - startPosition[1];
        // z = z - startPosition[2];

        // int xpos = (int) (x / boxSize);
        // int ypos = (int) (y / boxSize);
        // int zpos = (int) (z / boxSize);

        // return cellGrid[xpos,ypos,zpos];

        return cells[UnityEngine.Random.Range(0, cells.Count)];

    }

    public override Cell[] GetNeighbours(Cell cell){
        return cell.neighbours.ToArray();
    }

    public override List<Vector3> PathToVectors(List<Cell> generatedPath){

        List<Vector3> vectorPath = new List<Vector3>();

        foreach(Cell cell in generatedPath){
            vectorPath.Add(floattovec3(cell.floatPosition));
        }

        return vectorPath;
    }

    public override Vector3 CellToVector(Cell cell){

        return floattovec3(cell.floatPosition);
    }

    private float[] vec3tofloat(Vector3 input){
        return new float[] {input[0], input[1], input[2]};
        
    }

    private Vector3 floattovec3(float[] input){
        return new Vector3 (input[0], input[1], input[2]);
        
    }
}
