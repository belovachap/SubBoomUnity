using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public int totalGamesPlayed, lastScore, highScore;
    public float totalSecondsPlayed;
    public string lastScoreDateTime = "", highScoreDateTime = "";

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
        public int totalGamesPlayed = 0;
        public float totalSecondsPlayed = 0;

        public int lastScore = 0;
        public string lastScoreDateTime = "";

        public int highScore = 0;
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

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";

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
    }
}
