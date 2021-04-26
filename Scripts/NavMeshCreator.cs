using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*

Creates a graph from bounding boxes of objects (if it works)

*/

public class NavMeshCreator : MonoBehaviour{

    // Find all objects with obstacle tag

    private List<Vector3> points = new List<Vector3>();
    public LayerMask layerMask;
    public float offset;


    void Start(){

        int startTime = Environment.TickCount;
            

        var objects = GameObject.FindGameObjectsWithTag("Obstacle");
        var objectCount = objects.Length;
        foreach (var obj in objects) {
            Vector3 center = obj.GetComponent<Collider>().bounds.center;
            Vector3 size = obj.GetComponent<Collider>().bounds.size;
            // Debug.Log(center);
            // Debug.Log(size);

            // Vector3 topleft = center + size/2;
            // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // // cube.transform.localScale =size;
            // cube.transform.position = topleft;
            // cube.GetComponent<Renderer>().material.color = Color.red;

            Vector3 boundPoint1 = obj.GetComponent<Collider>().bounds.min;
            Vector3 boundPoint2 = obj.GetComponent<Collider>().bounds.max;

            Vector3[] boundPoints = new Vector3[] {
             new Vector3(boundPoint1.x-offset, boundPoint1.y-offset, boundPoint1.z-offset),
             new Vector3(boundPoint2.x+offset, boundPoint2.y+offset, boundPoint2.z+offset),
             new Vector3(boundPoint1.x-offset, boundPoint1.y-offset, boundPoint2.z+offset),
             new Vector3(boundPoint1.x-offset, boundPoint2.y+offset, boundPoint1.z-offset),
             new Vector3(boundPoint2.x+offset, boundPoint1.y-offset, boundPoint1.z-offset),
             new Vector3(boundPoint1.x-offset, boundPoint2.y+offset, boundPoint2.z+offset),
             new Vector3(boundPoint2.x+offset, boundPoint1.y-offset, boundPoint2.z+offset),
             new Vector3(boundPoint2.x+offset, boundPoint2.y+offset, boundPoint1.z-offset)};


            foreach(Vector3 point in boundPoints){

                points.Add(point);

            
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localScale = new Vector3(.1f, .1f, .1f);
                cube.transform.position = point;
                cube.GetComponent<Renderer>().material.color = Color.red;
            

            }


        }

        int possibleTriangles = 0;
        int clearTriangles = 0;

        // Iterate all possible tetrahedra
        for (int i=0; i<points.Count;i++){
            for (int j=0; j<points.Count;j++){
                for (int k=0; k<points.Count;k++){
                    for (int l=0; l<points.Count;l++){

                        possibleTriangles++;


                        if (pathClear(points[i], points[j]) &&
                            pathClear(points[i], points[k]) &&
                            pathClear(points[i], points[l]) &&
                            pathClear(points[j], points[k]) &&
                            pathClear(points[j], points[l]) &&
                            pathClear(points[k], points[l]))
                        {

                            clearTriangles++;

                            
                            

                        }








                    }
                }
            }

        }

        Debug.Log(possibleTriangles);
        Debug.Log(clearTriangles);

        // // Iterate through each point and try to connect to each other point

        // bool[,] connectedGrid = new bool[points.Count, points.Count];

        

        // int numedges = 0;
        // for(int i=0; i<points.Count; i++){
        //     Vector3 point = points[i];
        //     for(int j=0; j<points.Count; j++){
        //         if (i==j){continue;}
        //         RaycastHit hit;
        //         if (Physics.Raycast(points[i], points[j] - points[i], out hit, Vector3.Distance(points[i], points[j])))
        //             {
        //                 // Debug.DrawRay(points[i], points[j] - points[i] * hit.distance, Color.yellow, 100, false);
        //                 // Debug.Log("Did Hit");
        //                 connectedGrid[i, j] = false;
        //             }
        //             else
        //             {
        //                 // Debug.DrawRay(points[i], points[j] - points[i], Color.white, 100, false);
        //                 connectedGrid[i, j] = true;
        //                 // Debug.Log("Did not Hit");
        //                 numedges++;
        //             }
        //     }
        // }






    //     int endTime = Environment.TickCount;
    //     Debug.Log(string.Format("Time to load: {0}", endTime-startTime));
    //     Debug.Log(String.Format("Num edges: {0}", numedges));


    //     PointGraph graph = new PointGraph();

    //     graph.initialize(points, connectedGrid);

    //     graph.createGraph();






    //     Astar astar = new Astar(graph);

    //         float[] floatorigin = new float[] {0, 0, 0};
    //         float[] floattarget = new float[] {1, 1, 1};

    //         startTime = Environment.TickCount;
    //         var answer = astar.search(floatorigin, floattarget);
    //         endTime = Environment.TickCount;
    //         Debug.Log(string.Format("Time to search: {0}", endTime-startTime));

    //         List<Vector3> vectorPath;

    //         if (answer == null){
    //             Debug.Log("ASTAR: No valid path");
    //             vectorPath = null;
    //         }
    //         else{
    //             startTime = Environment.TickCount;
    //             vectorPath = astar.PathToVectors();
    //             endTime = Environment.TickCount;
    //             Debug.Log(string.Format("Time to convert to vector: {0}", endTime-startTime));
                
    //         }

    //         foreach(Vector3 vec in vectorPath){
    //             // Debug.Log(vec.ToString());
    //             GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //             cube.transform.localScale = new Vector3(1f, 1f, 1f);
    //             cube.transform.position = vec;
    //             cube.GetComponent<Renderer>().material.color = Color.green;
    //         }

 

    //             for (int i=1; i<vectorPath.Count; i++){
    //                 Debug.DrawRay(vectorPath[i], vectorPath[i-1] - vectorPath[i], Color.green, 1000, false);
    //                 //Vector3.Distance(vectorPath[i], vectorPath[i-1])
    //             }
            

    //         Debug.Log(string.Format("StraightLineRatio: {0}", astar.StraightLineRatio()));






    }

    public bool pathClear(Vector3 point1, Vector3 point2){

        RaycastHit hit;
        return (Physics.Raycast(point1, point2 - point1, out hit, Vector3.Distance(point1, point2)));

    }
}
