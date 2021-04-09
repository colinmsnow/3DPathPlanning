using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


/* 

Stores the grid for a scene and associated methods


*/
[Serializable]
public class MapGrid{

    bool[,,] collisionMask;

    float[] startPosition;

    int[] size;
    float boxSize;
    public Cell[,,] cellGrid;

    public MapGrid(){
    }


    public void initialize(float[] _startPosition, int[] _size, float _boxSize){
        size = _size;
        startPosition = _startPosition;
        collisionMask = new bool[(int)size[0], (int)size[1], (int)size[2]];
        boxSize = _boxSize;
        // cellGrid = new Cell[(int)size[0], (int)size[1], (int)size[2]];
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
    public float euclideanDistanceCell(Cell one, Cell two){
        int[] a = one.position;
        int[] b = two.position;
        return Mathf.Sqrt((a[0]-b[0])*(a[0]-b[0]) + (a[1]-b[1])*(a[1]-b[1]) + (a[2]-b[2])*(a[2]-b[2])) * boxSize;
    }

    /* Gets manhattan distance between two boxes */
    public float manhattanDistance(int x1, int y1, int z1, int x2, int y2, int z2){
        return (Mathf.Abs(x1-x2) + Mathf.Abs(y1-y2) + Mathf.Abs(z1-z2) * boxSize);
    }

    public Cell[,,] createCellGrid(){
        Cell[,,] _cellGrid = new Cell[(int)size[0], (int)size[1], (int)size[2]];
        for(int i=0; i<size[0]; i++){
            for(int j=0; j<size[1]; j++){
                for(int k=0; k<size[2]; k++){
                    _cellGrid[i, j, k] = new Cell(collisionMask[i,j,k], i, j, k);

                    // Debug.Log(cellGrid[i, j, k]);
                
        }}}
        this.cellGrid = _cellGrid;
        return _cellGrid;
    }

    public Cell findCellByXYZ(float x, float y, float z){
        
        x = x - startPosition[0];
        y = y - startPosition[1];
        z = z - startPosition[2];

        int xpos = (int) (x / boxSize);
        int ypos = (int) (y / boxSize);
        int zpos = (int) (z / boxSize);

        // Debug.Log(xpos);
        // Debug.Log(ypos);
        // Debug.Log(zpos);
        // Debug.Log(cellGrid.GetLength(0));
        // Debug.Log(cellGrid.GetLength(1));
        // Debug.Log(cellGrid.GetLength(2));
        // Debug.Log(cellGrid[xpos,ypos,zpos]);
        // Debug.Log(cellGrid[(int)1,(int)1,(int)1]);
        // Debug.Log(cellGrid[xpos,ypos,zpos]);

        return cellGrid[xpos,ypos,zpos];
                // return cellGrid[1,1,1];

    }

    public void checkCell(){
        Debug.Log("Check");
        Debug.Log(this.cellGrid[(int)1,(int)1,(int)1]);
    }

    public Cell[] GetNeighbours(Cell cell){

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
                        catch (System.IndexOutOfRangeException ex){
                            result[resnum] = null;
                        }
                        resnum++;
                    }
            }}}

        return result;
    }

}
[Serializable]
public class Cell {

    public int[] position = new int[] {0, 0, 0};
    public bool empty;

    public float cost, heuristic;
    public float f
    {
        get { return cost + heuristic; }
    }

    public Cell parent = null;

    private Cell() { }

    public Cell(bool _empty, int _x, int _y, int _z){

        empty = _empty;
        position[0] = _x;
        position[1] = _y;
        position[2] = _z;

    }

    

}





public class GameStorage : MonoBehaviour {
 
     public static GameStorage storage;
     public float health;
     public float experience;
     //Note to self: access by GameStorage.storage.health etc
 
     public void Save(MapGrid data){
         BinaryFormatter bf = new BinaryFormatter();
         FileStream file = File.Create(Application.persistentDataPath + "/mapgrid.dat");
         
        //  playerData data = new playerData();
        //  data.health = health;
        //  data.experience = experience;
         
         bf.Serialize(file, data);
         file.Close();
     }
 
     public MapGrid Load(){
         if(File.Exists(Application.persistentDataPath + "/mapgrid.dat")){
             BinaryFormatter bf = new BinaryFormatter();
             FileStream file = File.Open(Application.persistentDataPath + "/mapgrid.dat", FileMode.Open);
             MapGrid data = (MapGrid)bf.Deserialize(file);
             file.Close();
             return data;
         }
         return null;
     }
}





