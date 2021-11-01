using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public delegate void StartGameDelegate();
    public static event StartGameDelegate GameStarted;

    public delegate void StopGameDelegate();
    public static event StopGameDelegate GameStopped;

    private bool gameRunning;

    private void OnEnable()
    {
        // setup event listeners
        HostController.HostDied += StopGame;
    }

    private void OnDisable()
    {
        // clear event listeners
        HostController.HostDied -= StopGame;
    }

    void Start()
    {
        // setup variables

        // start the game
        StartGame();
    }
    
    void Update()
    {

    }

    private void StartGame()
    {
        // fire the start event
        if (GameStarted != null) GameStarted();
        gameRunning = true;
    }

    private void StopGame(HostDeathReason deathReason)
    {
        if (GameStopped != null) GameStopped();
    }
}
