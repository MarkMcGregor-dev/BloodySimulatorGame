using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string PlayScene;
    public string InstructionsScene;
    public string BackScene;
    public string RetryScene;

    private Button startButton;
    private Button howToPlayButton;
    private Button backButton;
    private Button retryButton;


    void Start()
    {

    }

    public void StartPressed()
    {
        // go to the in-game scene when pressed
        SceneManager.LoadScene(PlayScene);
    }

    public void HowToPressed()
    {
        // go to the in-game scene when pressed
        SceneManager.LoadScene(InstructionsScene);
    }

    public void BackPressed()
    {
        // go to the in-game scene when pressed
        SceneManager.LoadScene(BackScene);
    }

    public void RetryPressed()
    {
        SceneManager.LoadScene(RetryScene);
    }
}
