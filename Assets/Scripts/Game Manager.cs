using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;

    [SerializeField] private GameObject gameOverScreen;

    private AudioSource source;
    [SerializeField] private AudioClip[] soundsList = new AudioClip[3];

    private int score;
    private float timePlayed;

    public bool IsGameActive {get; private set; }

    private void Start()
    {
        // sets game over screen to inactive
        gameOverScreen.SetActive(false);

        // loads high score if one is already found
        highScoreText.text = "High Score: " + GameData.Instance.highScore.ToString();

        // plays music for the game scene
        source = gameObject.GetComponent<AudioSource>();
        source.Play();

        // game is currently active, and the player can control the destroyer
        IsGameActive = true;
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

        source.clip = soundsList[1];
        source.loop = false;
        source.Play();

        SaveGameStats();
    }

    public void RestartClick()
    {
        PlayButtonSFX();

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void MainMenuClick()
    {
        PlayButtonSFX();

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

    private void PlayButtonSFX()
    {
        source.clip = soundsList[2];
        source.Play();
    }
}
