using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MainMenu : MonoBehaviour
{
    [SerializeField] private Text gamesPlayedText, secondsPlayedText, lastScoreText, highScoreText;

    public void Start() {
        GameData.Instance.Load();

        gamesPlayedText.text = "Games Played: " + GameData.Instance.totalGamesPlayed.ToString();
        secondsPlayedText.text = "Seconds Played: " + GameData.Instance.totalSecondsPlayed.ToString();

        if (GameData.Instance.lastScoreDateTime == "")
        {
            lastScoreText.text = "";
        }
        else
        {
            lastScoreText.text = "Last Score: " + GameData.Instance.lastScore.ToString() + " on " + GameData.Instance.lastScoreDateTime;
        }

        if (GameData.Instance.highScoreDateTime == "")
        {
            highScoreText.text = "";
        }
        else
        {
            highScoreText.text = "High Score: " + GameData.Instance.highScore.ToString() + " on " + GameData.Instance.highScoreDateTime;
        }
    }

    public void StartButtonClick()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void QuitButtonClick()
    {
        GameData.Instance.Save();

        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
