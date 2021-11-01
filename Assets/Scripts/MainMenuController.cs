using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string InGameSceneName;

    private Button startButton;

    void Start()
    {
        // setup variables
        startButton = GameObject.Find("Start Button").GetComponent<Button>();
        startButton.onClick.AddListener(StartPressed);
    }

    private void StartPressed()
    {
        // go to the in-game scene when pressed
        SceneManager.LoadScene(InGameSceneName);
    }
}
