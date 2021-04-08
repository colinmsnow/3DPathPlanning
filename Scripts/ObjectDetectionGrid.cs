using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetectionGrid : MonoBehaviour
{

    public Vector3 startPosition;

    public float numBoxes;
    public float boxSize;
    // Start is called before the first frame update
    void Start()
    {
        for (int x=0; x<numBoxes; x++){
            for (int y=0; y<numBoxes; y++){
                for (int z=0; z<numBoxes; z++){
                    // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    // cube.transform.position = new Vector3(x*boxSize, y*boxSize, z*boxSize);
                    // // cube.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV();
                    // cube.GetComponent<Renderer>().material.color = Color.black;

                    Collider[] hitColliders = Physics.OverlapSphere(new Vector3(x*boxSize + startPosition[0], y*boxSize+ startPosition[1], z*boxSize+ startPosition[2]), boxSize);
                    if (hitColliders.Length > 1){
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = new Vector3(x*boxSize + startPosition[0], y*boxSize+ startPosition[1], z*boxSize+ startPosition[2]);
                        cube.GetComponent<Renderer>().material.color = Color.red;

                    }



                    // {Debug.Log(hitColliders);};
                    // Debug.Log(hitColliders.Length);
                }
            }
        }
        // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // cube.transform.position = new Vector3(0, 0.5f, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
