using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Text gamesPlayedText;

    [SerializeField]
    Text secondsPlayedText;

    [SerializeField]
    Text lastScoreText;

    [SerializeField]
    Text highScoreText;

    public void Start()
    {
        GameData gd = GameDataFileHandler.Load();

        gamesPlayedText.text = "Games Played: " + gd.totalGamesPlayed.ToString();
        secondsPlayedText.text = "Seconds Played: " + gd.totalSecondsPlayed.ToString();

        if (gd.lastScoreDateTime == "")
        {
            lastScoreText.text = "";
        }
        else
        {
            lastScoreText.text = "Last Score: " + gd.lastScore.ToString() + " on " + gd.lastScoreDateTime;
        }

        if (gd.highScoreDateTime == "")
        {
            highScoreText.text = "";
        }
        else
        {
            highScoreText.text = "High Score: " + gd.highScore.ToString() + " on " + gd.highScoreDateTime;
        }
    }

    public void QuitButtonClick()
    {
        Application.Quit();
    }
    public void StartButtonClick()
    {
        SceneManager.LoadScene("SubBoomScene", LoadSceneMode.Single);
    }
}