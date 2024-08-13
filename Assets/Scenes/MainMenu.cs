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

    [SerializeField]
    RawImage artworkRawImage;

    public void Start() {
        GameData gd = GameDataFileHandler.Load();

        gamesPlayedText.text = "Games Played: " + gd.totalGamesPlayed.ToString();
        secondsPlayedText.text = "Seconds Played: " + gd.totalSecondsPlayed.ToString();

        if (gd.lastScoreDateTime == "") {
            lastScoreText.text = "";
        }
        else {
            lastScoreText.text = "Last Score: " + gd.lastScore.ToString() + " on " + gd.lastScoreDateTime;
        }

        if (gd.highScoreDateTime == "") {
            highScoreText.text = "";
        }
        else {
            highScoreText.text = "High Score: " + gd.highScore.ToString() + " on " + gd.highScoreDateTime;
        }

        string[] artwork = {
            "sub_boom_art_one",
            "sub_boom_art_two",
            "sub_boom_art_three",
            "sub_boom_art_four"
        };
        artworkRawImage.texture = Resources.Load<Texture2D>(artwork[Random.Range(0, 4)]);
    }

    public void QuitButtonClick ()
    {
        Application.Quit();
    }

    public void StartButtonClick ()
    {
        SceneManager.LoadScene("SubBoomScene", LoadSceneMode.Single);
    }
}
