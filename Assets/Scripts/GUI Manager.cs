using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private PlayerController playerController;

    private ulong score = 0;
    private float timePlayed = 0.0f;

    // GameData gd = GameData.Instance;

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        gameOverScreen.SetActive(false);

        highScoreText.text = "High Score: " + GameData.Instance.highScore.ToString();
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

        if (score >= GameData.Instance.highScore)
        {
            highScoreText.text = "High Score: " + score.ToString();
        }
    }

    public void GameOverScreen()
    {
        if (!gameOverScreen.activeSelf)
        {
            gameOverScreen.SetActive(true);
            playerController.isGameActive = false;

            SaveGameStats();
        }
    }

    public void RestartClick()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void MainMenuClick()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private void SaveGameStats()
    {
        DateTime now = DateTime.Now;

        GameData.Instance.totalGamesPlayed += 1;
        GameData.Instance.totalSecondsPlayed += (ulong)timePlayed;

        GameData.Instance.lastScore = score;
        GameData.Instance.lastScoreDateTime = now.ToString();

        if (score >= GameData.Instance.highScore)
        {
            GameData.Instance.highScore = score;
            GameData.Instance.highScoreDateTime = now.ToString();
        }

        GameData.Instance.Save();
    }

    public void QuitButtonClick()
    {
        #if UNITY_EDITOR
             EditorApplication.ExitPlaymode();
        #else
             Application.Quit();
        #endif
    }
}
