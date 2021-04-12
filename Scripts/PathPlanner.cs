using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*

Simple path planning object. Can be used to visualize paths.

Inputs:
    Origin: Place to start search
    Target: Target position
    
Loads:
    A graph object from saved file


Returns:
    Nothing but shows path in green boxes

*/

public class PathPlanner : MonoBehaviour{


    public Vector3 origin;
    public Vector3 target;
    private Graph mapgrid;


    void Update()
    {

        if (Input.GetKeyDown("space")){
            Debug.Log("pressed space");
            GraphStorage storage = gameObject.AddComponent( typeof(GraphStorage)) as GraphStorage;

            int startTime = Environment.TickCount;
            mapgrid = storage.Load();
            int endTime = Environment.TickCount;
            Debug.Log(string.Format("Time to load: {0}", endTime-startTime));

            Debug.Log(mapgrid);

            startTime = Environment.TickCount;
            Cell[,,] graph = mapgrid.createGraph();
            endTime = Environment.TickCount;
            Debug.Log(string.Format("Time to create grid: {0}", endTime-startTime));

            Astar astar = new Astar(mapgrid);

            float[] floatorigin = new float[] {origin[0], origin[1], origin[2]};
            float[] floattarget = new float[] {target[0], target[1], target[2]};

            startTime = Environment.TickCount;
            var answer = astar.search(floatorigin, floattarget);
            endTime = Environment.TickCount;
            Debug.Log(string.Format("Time to search: {0}", endTime-startTime));

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
                // Debug.Log(vec.ToString());
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localScale = new Vector3(.5f, .5f, .5f);
                cube.transform.position = vec;
                cube.GetComponent<Renderer>().material.color = Color.green;
            }

            Debug.Log(string.Format("StraightLineRatio: {0}", astar.StraightLineRatio()));

        }
    }
}