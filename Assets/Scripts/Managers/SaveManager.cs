using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    //singleton
    public static SaveManager Instance { get; private set; }
    
    // path where the save file will be stored
    private string _savePath;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // set the save file path
        _savePath = Application.persistentDataPath + "/Save.Json";
        Debug.Log("save path : " + _savePath);
    }

    public void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_savePath, json);
        Debug.Log("Game saved !");
    }

    public GameData Load()
    {
        if (!File.Exists(_savePath))
        {
            Debug.Log("no save file found, creat a new one !");
            return new GameData();
        }

        string json = File.ReadAllText(_savePath);
        GameData data = JsonUtility.FromJson<GameData>(json);
        Debug.Log("Game Loaded !");
        return data;
    }
}
