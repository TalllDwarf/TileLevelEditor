using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

public class SaveLoadManager
{

    private static SaveLoadManager instance = null;

    private SaveLoadManager() { }

    public static SaveLoadManager Instance
    {
        get
        {
            if (instance == null)
                instance = new SaveLoadManager();

            return instance;
        }
    }

    //Returns the name of all the levels
    public string[] GetListOfLevels()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.level");
        
        for(int i = 0; i < files.Length; i++)
        {
            files[i] = Path.GetFileNameWithoutExtension(files[i]);
        }

        return files;
    }

    //Save data to file
    public void Save <T>(T data, string filename)
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + "/" + filename + ".lvl");

        bf.Serialize(file, data);
        file.Close();
    }

    public T Load<T> (string filename)
    {
        T data = default(T);

        if (File.Exists(Application.persistentDataPath + "/" + filename + ".lvl"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + filename + ".lvl", FileMode.Open);

            data = (T)bf.Deserialize(file);
            file.Close();
        }

        return data;
    }

    public void SaveList<T>(List<T> mapTiles, string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
        FileStream file = File.Create(Application.persistentDataPath + "/" + filename + ".level");
        serializer.Serialize(file, mapTiles);
    }

    public List<T> LoadList<T>(string filename)
    {
        XmlSerializer deSerializer = new XmlSerializer(typeof(List<T>));

        TextReader reader = new StreamReader(Application.persistentDataPath + "/" + filename + ".level");

        object loadedList = deSerializer.Deserialize(reader);

        reader.Close();

        return (List<T>)loadedList;
    }
}
