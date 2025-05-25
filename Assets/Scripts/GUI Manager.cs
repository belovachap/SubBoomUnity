using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private PlayerController playerController;

    private ulong score = 0;
    private float timePlayed = 0.0f;

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        gameOverScreen.SetActive(false);
    }

    private void Update()
    {
        if (playerController.isGameActive)
        {
            timePlayed += Time.deltaTime;
        }
    }

    public void AddScore(int incAmount)
    {
        score = (ulong)incAmount;
        scoreText.text = "Score: " + score.ToString();
    }

    public void GameOverScreen()
    {
        if (!gameOverScreen.activeSelf)
        {
            gameOverScreen.SetActive(true);
            playerController.isGameActive = false;
        }
    }

    public void RestartClick()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void MainMenuClick()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    void SaveGameStats()
    {
        GameData gd = GameDataFileHandler.Load();
        DateTime now = DateTime.Now;

        gd.totalGamesPlayed += 1;
        gd.totalSecondsPlayed += (ulong)timePlayed;
        gd.lastScore = score;
        gd.lastScoreDateTime = now.ToString();

        if (score >= gd.highScore)
        {
            gd.highScore = score;
            gd.highScoreDateTime = now.ToString();
        }

        GameDataFileHandler.Save(gd);
    }

    void OnApplicationQuit()
    {
        if (playerController.isGameActive)
        {
            SaveGameStats();
        }
    }
}
