using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/*
    Saves and loads a graph object from memory

    Currently just gives it a default name and loads the last one it saved
*/

public class GraphStorage : MonoBehaviour {
 
    public static GraphStorage storage;
 
    public void Save(Graph data){
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/mapgrid.dat");
        bf.Serialize(file, data);
        file.Close();
    }
 
    public Graph Load(){
        if(File.Exists(Application.persistentDataPath + "/mapgrid.dat")){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/mapgrid.dat", FileMode.Open);
            Graph data = (Graph)bf.Deserialize(file);
            file.Close();
            return data;
        }
        return null;
    }
}