using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] Text scoreText;
    [SerializeField] Text highScoreText;

    [Header("UI Screens")]
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject settingsScreen;

    private int score;
    private float timePlayed;

    public bool IsGameActive {get; private set; }
    private bool isSettingsOpen = false;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        // game is currently active, and the player can control the destroyer
        IsGameActive = true;

        // sets game over screen and settings screen to inactive for precautionary measure
        gameOverScreen.SetActive(false);
        settingsScreen.SetActive(false);

        // plays in-game music
        audioManager.PlayMusic(audioManager.inGameMusic);

        // loads high score if one is already found
        highScoreText.text = "High Score: " + GameData.Instance.highScore.ToString();
    }

    private void Update()
    {
        if (IsGameActive)
        {
            timePlayed += Time.deltaTime;
        }
    }

    public void UpdateScore(int incAmount)
    {
        score += incAmount;
        scoreText.text = "Score: " + score.ToString();

        if (score >= GameData.Instance.highScore)
        {
            highScoreText.text = "High Score: " + score.ToString();
        }
    }

    public void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        IsGameActive = false;

        audioManager.PlaySFX(audioManager.gameOverSFX);

        SaveGameStats();
    }

    public void SettingsScreen()
    {
        if (!isSettingsOpen)
        {
            IsGameActive = false;
            settingsScreen.SetActive(true);

            isSettingsOpen = true;
        }
        else
        {
            IsGameActive = true;
            settingsScreen.SetActive(false);

            isSettingsOpen = false;
        }
    }

    public void RestartClick()
    {
        audioManager.PlaySFX(audioManager.buttonPressSFX);

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void MainMenuClick()
    {
        audioManager.PlaySFX(audioManager.buttonPressSFX);

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private void SaveGameStats()
    {
        DateTime now = DateTime.Now;

        GameData.Instance.totalGamesPlayed += 1;
        GameData.Instance.totalSecondsPlayed += timePlayed;

        GameData.Instance.lastScore = score;
        GameData.Instance.lastScoreDateTime = now.ToString();

        if (score >= GameData.Instance.highScore)
        {
            GameData.Instance.highScore = score;
            GameData.Instance.highScoreDateTime = now.ToString();
        }

        GameData.Instance.Save();
    }
}
