using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Playables;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        //string fullPath = Path.Combine(dataDirPath, dataFileName);
        string fullPath = dataDirPath + dataFileName;
        Debug.Log(fullPath);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            //try 
            //{
            //    string dataToLoad = "";
            //    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
            //    {
            //        using (StreamReader reader = new StreamReader(stream))
            //        {
            //            dataToLoad = reader.ReadToEnd();
            //        }
            //    }
            //    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            //} 
            //catch (Exception e) 
            //{ 
            //    Debug.LogError("Error ocurred when trying to save data to file: " + fullPath + "\n" + e); 
            //}
            string json = File.ReadAllText(fullPath);
            loadedData = JsonUtility.FromJson<GameData>(json);
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        //string fullPath = Path.Combine(dataDirPath, dataFileName);
        string fullPath = dataDirPath + dataFileName;
        string dataToStore = JsonUtility.ToJson(data, true);
        File.WriteAllText(fullPath, dataToStore);
        Debug.Log("SAVE GAMEDATA SUCCESSFULLY!" + dataToStore);

        //try
        //{
        //    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        //    string dataToStore = JsonUtility.ToJson(data, true);
        //    using (FileStream stream = new FileStream(fullPath, FileMode.Create))
        //    {
        //        using (StreamWriter writer = new StreamWriter(stream))
        //        {
        //            writer.Write(dataToStore);
        //        }
        //    }
        //}
        //catch (Exception e)
        //{
        //    Debug.LogError("Error ocurred when trying to save data to file: " + fullPath + "\n" + e);
        //}
    }

    public void DeleteData()
    {
        //string fullPath = Path.Combine(dataDirPath, dataFileName);
        string fullPath = dataDirPath + dataFileName;

        if (!File.Exists(fullPath))
        {
            Debug.Log("GAMEDATA IS NOT FOUND OR FILE IS ALREADY DELETED");
            return;
        }
        File.Delete(fullPath);
        Debug.Log("GAME DATA DELETED");
        GameManager.instance.ReloadScene();
    }
}
