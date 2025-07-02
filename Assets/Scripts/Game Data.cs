using System.IO;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public int totalGamesPlayed, totalSecondsPlayed, lastScore, highScore;
    public float masterVolume, musicVolume, soundVolume;
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
        public int totalSecondsPlayed = 0;

        public int lastScore = 0;
        public string lastScoreDateTime = "";

        public int highScore = 0;
        public string highScoreDateTime = "";

        public float masterVolume = 1;
        public float musicVolume = 1;
        public float soundVolume = 1;
    }

    public void Save()
    {
        SaveData data = new()
        {
            totalGamesPlayed = totalGamesPlayed,
            totalSecondsPlayed = totalSecondsPlayed,

            lastScore = lastScore,
            lastScoreDateTime = lastScoreDateTime,

            highScore = highScore,
            highScoreDateTime = highScoreDateTime,

            masterVolume = masterVolume,
            musicVolume = musicVolume,
            soundVolume = soundVolume
        };

        PlayerPrefs.SetString("Data", JsonUtility.ToJson(data));
    }

    public void Load()
    {
        var data = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("Data"));

        if (data != null)
        {
            totalGamesPlayed = data.totalGamesPlayed;
            totalSecondsPlayed = data.totalSecondsPlayed;

            lastScore = data.lastScore;
            lastScoreDateTime = data.lastScoreDateTime;

            highScore = data.highScore;
            highScoreDateTime = data.highScoreDateTime;

            masterVolume = data.masterVolume;
            musicVolume = data.musicVolume;
            soundVolume = data.soundVolume;
        }
    }
}
