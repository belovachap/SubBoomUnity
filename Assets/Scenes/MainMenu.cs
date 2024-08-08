using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void QuitButtonClick ()
    {
        Application.Quit();
    }

    public void StartButtonClick ()
    {
        SceneManager.LoadScene("SubBoomScene", LoadSceneMode.Single);
    }
}
