using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathPlanner : MonoBehaviour{


    public Vector3 origin;
    public Vector3 target;
    private MapGrid mapgrid;


    void Update()
    {

        if (Input.GetKeyDown("space")){
            Debug.Log("pressed space");
            GameStorage storage = gameObject.AddComponent( typeof(GameStorage)) as GameStorage;

            mapgrid = storage.Load();

            Debug.Log(mapgrid);

            Cell[,,] graph = mapgrid.createCellGrid();

            Astar astar = new Astar(mapgrid);

            float[] floatorigin = new float[] {origin[0], origin[1], origin[2]};
            float[] floattarget = new float[] {target[0], target[1], target[2]};

            var answer = astar.search(floatorigin, floattarget);
            // Debug.Log(answer);

            List<Vector3> vectorPath;

            if (answer == null){
                Debug.Log("ASTAR: No valid path");
                vectorPath = null;
            }
            else{
                vectorPath = astar.PathToVectors();
            }

            foreach(Vector3 vec in vectorPath){
                Debug.Log(vec.ToString());
            }




            // Debug.Log(string.Format("ANSWER: {0},{1},{2}", answer.position[0], answer.position[1], answer.position[2]));
        }






    }
}