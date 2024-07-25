using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//using System.IO;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance { get; private set; }

    [Header("File Storage Config")]
    [SerializeField] private string fileName = "/BlockBusterData.json";
    string gameDataPath;
    private GameData gameData;
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler dataHandler;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        //gameDataPath = Application.persistentDataPath + "/GameData.json";
        //LoadGame();
        //Debug.Log(gameDataPath);
    }

    private void Start()
    {
        gameDataPath = Application.persistentDataPath;
        dataHandler = new FileDataHandler(gameDataPath, fileName);
        _dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        //if (!File.Exists(gameDataPath))
        //{
        //    Debug.Log("SAVES NOT FOUND TO LOAD, NEED TO SAVE AT LEAST ONES!");
        //    return;
        //}
        //string json = File.ReadAllText(gameDataPath);
        //gameData = JsonUtility.FromJson<GameData>(json);

        gameData = dataHandler.Load();

        if (gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        //string json = JsonUtility.ToJson(gameData, true);
        //File.WriteAllText(gameDataPath, json);

        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    public void DeleteGameData()
    {
        dataHandler.DeleteData();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            SaveGame();
        }
    }
}