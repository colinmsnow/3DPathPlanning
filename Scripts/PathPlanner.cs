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

            int startTime = Environment.TickCount;
            mapgrid = storage.Load();
            int endTime = Environment.TickCount;
            Debug.Log(string.Format("Time to load: {0}", endTime-startTime));

            Debug.Log(mapgrid);

            startTime = Environment.TickCount;
            Cell[,,] graph = mapgrid.createCellGrid();
            endTime = Environment.TickCount;
            Debug.Log(string.Format("Time to create grid: {0}", endTime-startTime));

            Astar astar = new Astar(mapgrid);

            float[] floatorigin = new float[] {origin[0], origin[1], origin[2]};
            float[] floattarget = new float[] {target[0], target[1], target[2]};

            startTime = Environment.TickCount;
            var answer = astar.search(floatorigin, floattarget);
            endTime = Environment.TickCount;
            Debug.Log(string.Format("Time to search: {0}", endTime-startTime));
            // Debug.Log(answer);

            List<Vector3> vectorPath;

            if (answer == null){
                Debug.Log("ASTAR: No valid path");
                vectorPath = null;
            }
            else{
                startTime = Environment.TickCount;
                vectorPath = astar.PathToVectors();
                endTime = Environment.TickCount;
                Debug.Log(string.Format("Time to convert to vector: {0}", endTime-startTime));
                
            }

            foreach(Vector3 vec in vectorPath){
                Debug.Log(vec.ToString());
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localScale = new Vector3(mapgrid.boxSize, mapgrid.boxSize, mapgrid.boxSize);
                cube.transform.position = vec;
                cube.GetComponent<Renderer>().material.color = Color.green;
            }




            // Debug.Log(string.Format("ANSWER: {0},{1},{2}", answer.position[0], answer.position[1], answer.position[2]));
        }






    }
}