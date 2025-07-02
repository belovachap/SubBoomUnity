using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

// sets the script to be executed later than all default scripts
// this is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MainMenu : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] Text gamesPlayedText;
    [SerializeField] Text secondsPlayedText;
    [SerializeField] Text lastScoreText;
    [SerializeField] Text highScoreText;

    [Header("UI Screens")]
    [SerializeField] GameObject mainMenuScreen;
    [SerializeField] GameObject settingsScreen;
    [SerializeField] GameObject howToPlayScreen;
    [SerializeField] GameObject creditsScreen;

    [Header("Buttons")]
    [SerializeField] GameObject mainMenuButton;

    AudioManager audioManager;

    private void Awake()
    {
        GameData.Instance.Load();

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Start()
    {
        audioManager.PlayMusic(audioManager.menuMusic);

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
        audioManager.PlaySFX(audioManager.buttonPressSFX);

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void QuitButtonClick()
    {
        audioManager.PlaySFX(audioManager.buttonPressSFX);

        GameData.Instance.Save();

        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    public void SettingsClick()
    {
        audioManager.PlaySFX(audioManager.buttonPressSFX);

        mainMenuScreen.SetActive(false);

        mainMenuButton.SetActive(true);
        settingsScreen.SetActive(true);
    }

    public void HowToPlayClick()
    {
        audioManager.PlaySFX(audioManager.buttonPressSFX);

        mainMenuScreen.SetActive(false);

        mainMenuButton.SetActive(true);
        howToPlayScreen.SetActive(true);
    }

    public void CreditsClick()
    {
        audioManager.PlaySFX(audioManager.buttonPressSFX);

        mainMenuScreen.SetActive(false);

        mainMenuButton.SetActive(true);
        creditsScreen.SetActive(true);
    }

    public void MainMenuClick()
    {
        audioManager.PlaySFX(audioManager.buttonPressSFX);

        mainMenuScreen.SetActive(true);
        mainMenuButton.SetActive(false);

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
