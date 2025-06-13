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
    [Header("Text")]
    [SerializeField] private Text gamesPlayedText;
    [SerializeField] private Text secondsPlayedText;
    [SerializeField] private Text lastScoreText;
    [SerializeField] private Text highScoreText;

    [Header("UI Screens")]
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject howToPlayScreen;
    [SerializeField] private GameObject creditsScreen;

    private AudioSource[] sounds;
    private AudioSource music;
    private AudioSource buttonPressSFX;

    public void Start() {
        GameData.Instance.Load();

        MainMenuClick();

        // grabs all the audio source components in the game object
        sounds = gameObject.GetComponents<AudioSource>();

        // plays music for the main menu scene
        music = sounds[0];

        // gets the button press sound effect to play
        buttonPressSFX = sounds[1];

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
        buttonPressSFX.Play();
        music.Stop();

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void QuitButtonClick()
    {
        buttonPressSFX.Play();

        GameData.Instance.Save();

        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    public void SettingsClick()
    {
        buttonPressSFX.Play();
    }

    public void HowToPlayClick()
    {
        buttonPressSFX.Play();
    }

    public void CreditsClick()
    {
        buttonPressSFX.Play();

        mainMenuScreen.SetActive(false);
        creditsScreen.SetActive(true);
    }

    public void MainMenuClick()
    {
        buttonPressSFX.Play();

        mainMenuScreen.SetActive(true);

        if (settingsScreen.activeInHierarchy)
        {
            settingsScreen.SetActive(false);
        }
        else if (howToPlayScreen.activeInHierarchy)
        {
            howToPlayScreen.SetActive(false);
        }
        else if (creditsScreen.activeInHierarchy)
        {
            creditsScreen.SetActive(false);
        }
    }
}
