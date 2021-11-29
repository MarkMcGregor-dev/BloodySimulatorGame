using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string PlayScene;
    public string InstructionsScene;
    public string OptionsScene;
    public string BackScene;

    private Button startButton;
    private Button howToPlayButton;
    private Button optionsButton;
    private Button backButton;

    void Start()
    {
        // setup variables
        //startButton = GameObject.Find("Start Button").GetComponent<Button>();
        //startButton.onClick.AddListener(StartPressed);

        //howToPlayButton = GameObject.Find("HowTo Button").GetComponent<Button>();
        //howToPlayButton.onClick.AddListener(HowToPressed);

        //optionsButton = GameObject.Find("Options Button").GetComponent<Button>();
        //optionsButton.onClick.AddListener(OptionsPressed);

        //backButton = GameObject.Find("Back Button").GetComponent<Button>();
        //backButton.onClick.AddListener(BackPressed);

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

    public void OptionsPressed()
    {
        // go to the in-game scene when pressed
        SceneManager.LoadScene(OptionsScene);
    }
    public void BackPressed()
    {
        // go to the in-game scene when pressed
        SceneManager.LoadScene(BackScene);
    }
}
