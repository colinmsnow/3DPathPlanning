using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// using MapGrid;


// 500^3 boxes takes 68961 ms = 1.1 min


/* This script works but should be rewritten to take an object-based
    search instead of a brute force. Each object should find its
    local cube mesh and determine which boxes that surround it are
    invalid, then update only these to be non accessible. This makes
    the algorithm dependent on the number and size of objects but not
    the size of the search space.

    Need to do:

    1. Determine where grid points lay around an object
        (should be simple modulus operation)

    2. Get bounding box info about an object

    3. Find grid cells which are invalid based on the parameters

    4. Save all that into a global accessible mesh

*/

public class ObjectDetectionGrid : MonoBehaviour
{

    public Vector3 startPosition;

    public int numBoxes;
    public float boxSize;
    public float objectRadius;
    public LayerMask m_LayerMask;

    int numSamples;
    int sample;
    public int percentage;

    
    // Start is called before the first frame update
    void Start()
    {
        numSamples = numBoxes*numBoxes*numBoxes;
        sample = 0;
        int percent = numSamples/100;
        Debug.Log(percent);
        int startTime = Environment.TickCount;
        // bool[,,] collisionMask = new bool[(int)numBoxes, (int)numBoxes, (int)numBoxes];

        MapGrid grid = new MapGrid();
        float[] startpos = new float[] {startPosition[0],startPosition[1],startPosition[2]};
        int[] initsize = new int[] {(int)numBoxes,(int)numBoxes,(int)numBoxes,};
        grid.initialize(startpos, initsize, boxSize);
        for (int x=0; x<numBoxes; x++){
            for (int y=0; y<numBoxes; y++){
                for (int z=0; z<numBoxes; z++){
                    // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    // cube.transform.position = new Vector3(x*boxSize, y*boxSize, z*boxSize);
                    // // cube.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV();
                    // cube.GetComponent<Renderer>().material.color = Color.black;

                    // collisionMask[x, y, z] = false;

                    Collider[] hitColliders = Physics.OverlapBox(new Vector3(x*boxSize + startPosition[0], y*boxSize+ startPosition[1], z*boxSize+ startPosition[2]), new Vector3(objectRadius, objectRadius, objectRadius), Quaternion.identity, m_LayerMask);
                    // Collider[] hitColliders = Physics.OverlapSphere(new Vector3(x*boxSize + startPosition[0], y*boxSize+ startPosition[1], z*boxSize+ startPosition[2]), objectRadius, m_LayerMask);

                    if (hitColliders.Length > 0){
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.localScale = new Vector3(boxSize, boxSize, boxSize);
                        cube.transform.position = new Vector3(x*boxSize + startPosition[0], y*boxSize+ startPosition[1], z*boxSize+ startPosition[2]);
                        cube.GetComponent<Renderer>().material.color = Color.red;

                        // collisionMask[x, y, z] = true;
                        grid.setMaskItem(x, y, z, false);
                        // Debug.Log(collisionMask[x, y, z]);
                    }
                    else{
                        grid.setMaskItem(x, y, z, true);
                    }

                    sample++;

                    if (sample%percent == 0){
                        // percentage = sample / numSamples * 100;
                        Debug.Log((float)sample / (float)numSamples * 100);
                    }

                    // Debug.Log(collisionMask[x, y, z]);



                    // {Debug.Log(hitColliders);};
                    // Debug.Log(hitColliders.Length);
                }
            }
        }

        int endTime = Environment.TickCount;

        Debug.Log(endTime-startTime);


        GameStorage storage = gameObject.AddComponent( typeof(GameStorage)) as GameStorage;

        storage.Save(grid);
        Debug.Log(Application.persistentDataPath);



        Cell[,,] graph = grid.createCellGrid();

        // Debug.Log(graph[1,1,1]);


        // grid.findCellByXYZ(0, 0, 0);
        grid.checkCell();

        Debug.Log("Graph");

        Debug.Log(graph[1,1,1]);
        Debug.Log(grid.cellGrid[2,0,0]);

        // for(int i=0; i<50; i++){
        //     for(int j=0; i<; i++){
        //         for(int k=0; i<size[2]; i++){

        // Debug.Log(grid.cellGrid[0, 0, 0].empty);

        // for (int x=0; x<numBoxes; x++){
        //     for (int y=0; y<numBoxes; y++){
        //         for (int z=0; z<numBoxes; z++){

        //         Debug.Log(collisionMask[x, y, z]);

        //         }}}

        // Debug.Log(collisionMask[0, 0, 0]);
        // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // cube.transform.position = new Vector3(0, 0.5f, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}



