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
public class MapGrid: Graph{

    bool[,,] collisionMask;

    public float[] startPosition;

    int[] size;
    public float boxSize;
    public Cell[,,] cellGrid;

    public MapGrid(){
    }


    public void initialize(float[] _startPosition, int[] _size, float _boxSize){
        size = _size;
        startPosition = _startPosition;
        collisionMask = new bool[(int)size[0], (int)size[1], (int)size[2]];
        boxSize = _boxSize;
    }

    public void setMaskItem(int x, int y, int z, bool value){
        collisionMask[x, y, z] = value;
    }

    public bool getMaskItem(int x, int y, int z){
        return collisionMask[x, y, z];
    }

    /* Gets euclidean distance between two boxes */
    public float euclideanDistance(int x1, int y1, int z1, int x2, int y2, int z2){
        return Mathf.Sqrt((x1-x2)*(x1-x2) + (y1-y2)*(y1-y2) + (z1-z2)*(z1-z2)) * boxSize;
    }

    /* Gets euclidean distance between two cells */
    public override float Distance(Cell one, Cell two){
        int[] a = one.position;
        int[] b = two.position;
        // return (1 / FastInvSqrt((a[0]-b[0])*(a[0]-b[0]) + (a[1]-b[1])*(a[1]-b[1]) + (a[2]-b[2])*(a[2]-b[2]))) * boxSize;
        return Mathf.Sqrt((a[0]-b[0])*(a[0]-b[0]) + (a[1]-b[1])*(a[1]-b[1]) + (a[2]-b[2])*(a[2]-b[2])) * boxSize;
    }

    /* Gets manhattan distance between two boxes */
    public float manhattanDistance(int x1, int y1, int z1, int x2, int y2, int z2){
        return (Mathf.Abs(x1-x2) + Mathf.Abs(y1-y2) + Mathf.Abs(z1-z2) * boxSize);
    }


    public override Cell[,,] createGraph(){
        Cell[,,] _cellGrid = new Cell[(int)size[0], (int)size[1], (int)size[2]];
        for(int i=0; i<size[0]; i++){
            for(int j=0; j<size[1]; j++){
                for(int k=0; k<size[2]; k++){
                    _cellGrid[i, j, k] = new Cell(collisionMask[i,j,k], i, j, k);    
        }   }   }
        this.cellGrid = _cellGrid;
        return _cellGrid;
    }

    public override Cell findCellByXYZ(float x, float y, float z){
        
        x = x - startPosition[0];
        y = y - startPosition[1];
        z = z - startPosition[2];

        int xpos = (int) (x / boxSize);
        int ypos = (int) (y / boxSize);
        int zpos = (int) (z / boxSize);

        return cellGrid[xpos,ypos,zpos];

    }


    public void checkCell(){
        Debug.Log("Check");
        Debug.Log(this.cellGrid[(int)1,(int)1,(int)1]);
    }


    public override Cell[] GetNeighbours(Cell cell){

        Cell[] result = new Cell[26];

        int resnum = 0;

        for (int i=-1; i<=1; i++){
            for (int j=-1; j<=1; j++){
                for (int k=-1; k<=1; k++){
                    // Debug.Log(string.Format("{0}, {1}, {2} ", i, j, k));
                    if (!(i==0 && j==0 && k==0)){
                        try{
                            result[resnum] = cellGrid[cell.position[0] + i, cell.position[1] + j, cell.position[2] + k];
                        }
                        catch (System.IndexOutOfRangeException){
                            result[resnum] = null;
                        }
                        resnum++;
                    }
            }}}

        return result;
    }

    public override List<Vector3> PathToVectors(List<Cell> generatedPath){

        List<Vector3> vectorPath = new List<Vector3>();
        Vector3 startpos = new Vector3(startPosition[0], startPosition[1], startPosition[2]);


        foreach(Cell cell in generatedPath){
            Vector3 boxpos = new Vector3(cell.position[0], cell.position[1], cell.position[2]);
            boxpos *=boxSize;
            boxpos = boxpos + startpos;
            vectorPath.Add(boxpos);
        }
        return vectorPath;
    }

    public override Vector3 CellToVector(Cell cell){
    
        Vector3 startpos = new Vector3(startPosition[0], startPosition[1], startPosition[2]);
        Vector3 boxpos = new Vector3(cell.position[0], cell.position[1], cell.position[2]);
        boxpos *=boxSize;
        boxpos = boxpos + startpos;
        return boxpos;
    }

    // [StructLayout(LayoutKind.Explicit)]
    // struct intfloatunion {
    //     [FieldOffset(0)]
    //     public float f;
    //     [FieldOffset(0)]
    //     public int i;
    // }
 
    // private float FastInvSqrt(float num){
    //         intfloatunion ifu = new intfloatunion();
    //         ifu.f = num;
    //         ifu.i = 1597463174 - (ifu.i >> 1);
    //         return ifu.f * (1.5f - (0.5f * ifu.f * ifu.f * ifu.f));
    //     }

}















