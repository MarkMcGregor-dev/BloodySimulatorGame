using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public Text scoreValue;

    public float totalScore;
    private float bloodCellScore = 1;
    private float blockageScore = 5;

    private void OnEnable()
    {
        // subscribes to collected blood cell event when it occurs in PlayerController class
        PlayerController.PlayerCollectedBloodCell += OnPlayerCollectedBloodCell;
        BlockageBehaviour.BlockageBroken += OnBlockageBroken;
    }

    private void OnDisable()
    {        
        // unsubscribes to collected blood cell event after it has occured
        PlayerController.PlayerCollectedBloodCell -= OnPlayerCollectedBloodCell;
        BlockageBehaviour.BlockageBroken -= OnBlockageBroken;
    }

    // listens to collected blood cell "announcement"
    // when the event is heart, this function is called:
    private void OnPlayerCollectedBloodCell()
    {
        totalScore = totalScore + bloodCellScore;
        scoreValue.text = "" + totalScore;
    }
    
    // this will be used when the player collides with blockages
    private void OnBlockageBroken()
    {
        totalScore = totalScore + blockageScore;
        scoreValue.text = "" + totalScore;
    }
}
