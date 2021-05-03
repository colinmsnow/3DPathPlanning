using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// Create grid of boxes given parameters and turn it into a graph


// 500^3 boxes takes 68961 ms = 1.1 min


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
                    Collider[] hitColliders = Physics.OverlapBox(new Vector3(x*boxSize + startPosition[0], y*boxSize+ startPosition[1], z*boxSize+ startPosition[2]), new Vector3(objectRadius, objectRadius, objectRadius), Quaternion.identity, m_LayerMask);

                    if (hitColliders.Length > 0){
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.localScale = new Vector3(boxSize, boxSize, boxSize);
                        cube.transform.position = new Vector3(x*boxSize + startPosition[0], y*boxSize+ startPosition[1], z*boxSize+ startPosition[2]);
                        cube.GetComponent<Renderer>().material.color = Color.red;

                        grid.setMaskItem(x, y, z, false);
                    }
                    else{
                        grid.setMaskItem(x, y, z, true);
                    }

                    sample++;
                }
            }
        }

        int endTime = Environment.TickCount;

        Debug.Log(endTime-startTime);


        GraphStorage storage = gameObject.AddComponent( typeof(GraphStorage)) as GraphStorage;

        storage.Save(grid);
        Debug.Log(Application.persistentDataPath);
        
    }
}



