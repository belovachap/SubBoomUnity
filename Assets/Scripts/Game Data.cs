using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    public SaveData data;
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

        SceneManager.activeSceneChanged += OnActiveSceneChanged;

        Load();
    }

    [System.Serializable]
    public class SaveData
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
        Debug.Log("Saving Game Data");
        PlayerPrefs.SetString("Data", JsonUtility.ToJson(data));
    }

    public void Load()
    {
        Debug.Log("Loading Game Data");
        data = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("Data"));

        if (data == null)
        {
            data = new SaveData();
        }
    }

    void OnApplicationQuit()
    {
        Save();
    }

    void OnActiveSceneChanged(Scene current, Scene next)
    {
        Save();
    }
}
