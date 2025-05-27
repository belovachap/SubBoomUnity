using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditorInternal;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public ulong totalGamesPlayed = 0;
    public ulong totalSecondsPlayed = 0;

    public ulong lastScore = 0;
    public string lastScoreDateTime = "";

    public ulong highScore = 0;
    public string highScoreDateTime = "";

    public static GameData Instance { get; set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Load();
    }


    [System.Serializable]
    class SaveData
    {
        public ulong totalGamesPlayed = 0;
        public ulong totalSecondsPlayed = 0;

        public ulong lastScore = 0;
        public string lastScoreDateTime = "";

        public ulong highScore = 0;
        public string highScoreDateTime = "";
    }

    public void Save()
    {
        SaveData data = new SaveData();

        data.totalGamesPlayed = totalGamesPlayed;
        data.totalSecondsPlayed = totalSecondsPlayed;

        data.lastScore = lastScore;
        data.lastScoreDateTime = lastScoreDateTime;

        data.highScore = highScore;
        data.highScoreDateTime = highScoreDateTime;

        string jsonData = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "gamedata.json", jsonData);

        /*
        string filePath = Path.Combine(Application.persistentDataPath, "gamedata.json");
        try
        {
            Directory.CreateDirectory(Application.persistentDataPath);
            string jsonData = JsonUtility.ToJson(data, true);
            using (FileStream stream = new(filePath, FileMode.Create))
            {
                using (StreamWriter writer = new(stream))
                {
                    writer.Write(jsonData);
                }
            }
        }

        catch (Exception e)
        {
            Debug.LogError("Error saving GameData object: " + e);
        }
        */
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "gamedata.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            totalGamesPlayed = data.totalGamesPlayed;
            totalSecondsPlayed = data.totalSecondsPlayed;

            lastScore = data.lastScore;
            lastScoreDateTime = data.lastScoreDateTime;

            highScore = data.highScore;
            highScoreDateTime = data.highScoreDateTime;
        }

        /*
        string filePath = Path.Combine(Application.persistentDataPath, "gamedata.json");

        try
        {
            string jsonData = "";
            using (FileStream stream = new(filePath, FileMode.Open))
            {
                using (StreamReader reader = new(stream))
                {
                    jsonData = reader.ReadToEnd();
                }
            }

            GameData data = JsonUtility.FromJson<GameData>(jsonData);
            totalGamesPlayed = data.totalGamesPlayed;
            totalSecondsPlayed = data.totalSecondsPlayed;

            lastScore = data.lastScore;
            lastScoreDateTime = data.lastScoreDateTime;

            highScore = data.highScore;
            highScoreDateTime = data.highScoreDateTime;
        }
         
        catch (Exception e)
        {
            Debug.Log("Error loading GameData object: " + e);
        }
        */
    }
}
