using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public Text scoreValue;

    public float totalScore;
    private float bloodCellScore = 1;
    private float blockageScore = 3;

    private void OnEnable()
    {
        // like and subscribe
        PlayerController.PlayerCollectedBloodCell += OnPlayerCollectedBloodCell;
    }

    private void OnDisable()
    {
        PlayerController.PlayerCollectedBloodCell -= OnPlayerCollectedBloodCell;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnPlayerCollectedBloodCell()
    {
        totalScore = totalScore + bloodCellScore;
        scoreValue.text = "Score: " + totalScore;
    }
    /*
    private void OnPlayerDestroyedBlockage()
    {
        totalScore = totalScore + blockageScore;
        scoreValue.text = "Score: " + totalScore;
    }*/
}
