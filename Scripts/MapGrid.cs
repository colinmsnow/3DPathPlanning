using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/* 

Stores the grid for a scene and associated methods


*/

public class MapGrid{

    bool[,,] collisionMask;
    Vector3 startPosition;
    Vector3 size;
    

    public MapGrid(Vector3 _startPosition, Vector3 _size){

        size = _size;
        startPosition - _startPosition;
        collisionMask = new bool[(int)size[0], (int)size[1], (int)size[2]];
    }

    public setMaskItem(int x, int y, int z, bool value){
        collisionMask[x, y, z] = value;
    }

}