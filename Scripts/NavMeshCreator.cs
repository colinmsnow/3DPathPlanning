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


        /* Delauny triangulation

        Defined as a set of tetrahedra with vertices in S such that the sphere defined by the set of
        vertices of each tetrahedron does not contain any nodes in S

        Steps:

        1. Create enclosing tetrahedron
        2. Add point from S
        3. Iterate through all spheres to check if the sphere contains the point
            3.1 If the sphere does not, keep the tetrahedron
            3.2 If it does, delete the tetrahedron and connect all points in the
                tetrahedron to the current point to create smaller terrahedra.
        4. Do this for all points in s.

        Runtime:

        Should run in O(n^5/3) for 3d but not sure how they check if points are in stuff etc.

        */


        Vector3 point1 = new Vector3(0,0,0);
        Vector3 point2 = new Vector3(1,1,1);
        Vector3 point3 = new Vector3(1,-3,4);
        Vector3 point4 = new Vector3(-2,2,1);
        Vector3 pointnew = new Vector3(-2,3,1);
        Vector3 pointnew2 = new Vector3(-.319f,1.94f,2.1f);

        List<Vector3> tetpoints = new List<Vector3>();
        tetpoints.Add(point1);
        tetpoints.Add(point2);
        tetpoints.Add(point3);
        tetpoints.Add(point4);
        tetpoints.Add(pointnew);
        tetpoints.Add(pointnew2);


        Vector3 point5 = new Vector3(6.25f,-5,-5);
        Vector3 point6 = new Vector3(5,5,5);
        Vector3 point7 = new Vector3(-5,5,0);
        Vector3 point8 = new Vector3(0,-5,5);

        List<Vector3> tetbounding = new List<Vector3>();
        tetbounding.Add(point5);
        tetbounding.Add(point6);
        tetbounding.Add(point7);
        tetbounding.Add(point8);


        // Create initial tetrahedron


        var initialtet = new Tetrahedron(point5, point6, point7, point8);

        // initialtet.display();

        List<Tetrahedron> tetrahedra = new List<Tetrahedron>();

        List<Tetrahedron> badTetrahedra = new List<Tetrahedron>();

        tetrahedra.Add(initialtet);


        foreach (Vector3 p in tetpoints){
            // Iterate through points adding one at a time

            badTetrahedra.Clear();

            Debug.Log("Starting first tetrahedron");


            foreach(Tetrahedron tet in tetrahedra){
                if (tet.contains(p)){
                    badTetrahedra.Add(tet);
                    // Debug.Log("Found bad tetrahedron");
                
                }
            }

            Debug.Log(String.Format("Number of bad tetrahedra: {0}", badTetrahedra.Count));

            List<Face> faces = new List<Face>();

            foreach(Tetrahedron tet in badTetrahedra){
                // Need to add triangle face which is not shared by any other tetrahedron

                var tetfaces = tet.faces();

                foreach(Face face in tetfaces){

                    bool present = false;

                    foreach (Face newface in faces){
                        if (newface.equals(face)){
                            present=true;
                        }
                    }

                    if (!present){
                        faces.Add(face);
                    }
                    // if (!faces.Contains (face)){
                    //     faces.Add(face);
                    //     // Debug.Log("Adding a face");
                    // }
                }
            }
            Debug.Log(String.Format("Number of faces: {0}", faces.Count));


            foreach(Face face in faces){

                // Create a new tetrahedron using this face and the test point
                var newtet = new Tetrahedron(p, face.vertices[0], face.vertices[1], face.vertices[2]);
                tetrahedra.Add(newtet);
                // Debug.Log("Created new tetrahedron");
            }

            foreach(Tetrahedron tet in badTetrahedra){
                // Want to remove that tetrahedron and create new ones to fill space

                // Remove
                tetrahedra.Remove(tet);

            }


            Debug.Log("Ending first tetrahedron");


        }



        // Delete tetrahedra which have one of the bounding vertices

        List<Tetrahedron> filteredTetrahedra = new List<Tetrahedron>();

        foreach(Tetrahedron tet in tetrahedra){
            if (!(tet.hasPoint(point5) || tet.hasPoint(point6) || tet.hasPoint(point7) || tet.hasPoint(point8))){
                filteredTetrahedra.Add(tet);
            }
        }

        foreach(Tetrahedron tet in filteredTetrahedra){
            tet.display();
        }

        Debug.Log(String.Format("Number of tetrahedra: {0}", filteredTetrahedra.Count));




























        // double[] point1 = new double[] {0, 0, 0};
        // double[] point2 = new double[] {1, 1, 1};
        // double[] point3 = new double[] {2, 5, 7};
        // double[] point4 = new double[] {3, -4, 6};
        

        // var solver = new CircumcentreSolver(point1, point2, point3, point4);

        // Debug.Log(solver.Centre[0]);
        // Debug.Log(solver.Centre[1]);
        // Debug.Log(solver.Centre[2]);
        // Debug.Log(solver.Radius);

        // Vector3 point1 = new Vector3(0,0,0);
        // Vector3 point2 = new Vector3(1,1,1);
        // Vector3 point3 = new Vector3(5,-3,6);
        // Vector3 point4 = new Vector3(-4,7,9);

        // List<Vector3> tetpoints = new List<Vector3>();
        // tetpoints.Add(point1);
        // tetpoints.Add(point2);
        // tetpoints.Add(point3);
        // tetpoints.Add(point4);


        foreach (Vector3 a in tetpoints){
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localScale = new Vector3(.1f, .1f, .1f);
                cube.transform.position = a;
                cube.GetComponent<Renderer>().material.color = Color.red;
        }

        foreach (Vector3 a in tetbounding){
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localScale = new Vector3(.1f, .1f, .1f);
                cube.transform.position = a;
                cube.GetComponent<Renderer>().material.color = Color.green;
        }

        // var tet = new Tetrahedron(point1, point2, point3, point4);


        // GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //         sphere.transform.localScale = new Vector3(tet.radius*2, tet.radius*2, tet.radius*2);
        //         sphere.transform.position = tet.center;
        //         sphere.GetComponent<Renderer>().material.color = Color.blue;




        
























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

public class Tetrahedron{

    public List<Vector3> vertices = new List<Vector3>();
    public Vector3 center;
    public float radius;

    public Tetrahedron(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 vertex4){
        vertices.Add(vertex1);
        vertices.Add(vertex2);
        vertices.Add(vertex3);
        vertices.Add(vertex4);


        var solver = new CircumcentreSolver(vec3todouble(vertex1), vec3todouble(vertex2), vec3todouble(vertex3), vec3todouble(vertex4));
        center = new Vector3((float) solver.Centre[0], (float) solver.Centre[1], (float) solver.Centre[2]);
        radius = (float) solver.Radius;
    }

    public List<Face> faces(){
        List<Face> facelist = new List<Face>();
        facelist.Add(new Face(vertices[0], vertices[1], vertices[2]));
        facelist.Add(new Face(vertices[0], vertices[1], vertices[3]));
        facelist.Add(new Face(vertices[0], vertices[2], vertices[3]));
        facelist.Add(new Face(vertices[1], vertices[2], vertices[3]));
        return facelist;
    }

    public bool contains(Vector3 point){
        return (Vector3.Distance(point, center) < radius);
    }

    public bool hasPoint(Vector3 point){
        return vertices.Contains(point);
    }

    public void display(){
        // Displays a tetrahedron

        foreach (Vector3 vertex in vertices){

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localScale = new Vector3(.2f, .2f, .2f);
                cube.transform.position = vertex;
                cube.GetComponent<Renderer>().material.color = Color.cyan;
        }

        Debug.DrawLine(vertices[0], vertices[1], Color.cyan, 1000, false);
        Debug.DrawLine(vertices[0], vertices[2], Color.cyan, 1000, false);
        Debug.DrawLine(vertices[0], vertices[3], Color.cyan, 1000, false);
        Debug.DrawLine(vertices[1], vertices[2], Color.cyan, 1000, false);
        Debug.DrawLine(vertices[1], vertices[3], Color.cyan, 1000, false);
        Debug.DrawLine(vertices[2], vertices[3], Color.cyan, 1000, false);
    }

    private Vector3 floattovec3(float[] input){
        return new Vector3 (input[0], input[1], input[2]);
        
    }

    private float[] vec3tofloat(Vector3 input){
        return new float[] {input[0], input[1], input[2]};
        
    }

    private Vector3 doubletovec3(double[] input){
        return new Vector3 ((float) input[0], (float) input[1], (float) input[2]);
        
    }

    private double[] vec3todouble(Vector3 input){
        return new double[] {(double) input[0], (double) input[1], (double) input[2]};
        
    }
}

public class Face{

    public List<Vector3> vertices = new List<Vector3>();

    public Face(Vector3 a, Vector3 b, Vector3 c){
        vertices.Add(a);
        vertices.Add(b);
        vertices.Add(c);
    }

    public bool equals(Face testface){
        return (vertices.Contains(testface.vertices[0]) && vertices.Contains(testface.vertices[1]) && vertices.Contains(testface.vertices[2]));
    }
}
