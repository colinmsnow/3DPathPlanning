using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GraphStorage : MonoBehaviour {
 
    public static GraphStorage storage;
 
    public void Save(MapGrid data){
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/mapgrid.dat");
        bf.Serialize(file, data);
        file.Close();
    }
 
    public MapGrid Load(){
        if(File.Exists(Application.persistentDataPath + "/mapgrid.dat")){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/mapgrid.dat", FileMode.Open);
            MapGrid data = (MapGrid)bf.Deserialize(file);
            file.Close();
            return data;
        }
        return null;
    }
}