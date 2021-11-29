using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour
{

    public Text scoreValue;
    private float finalScoreValue;


    // Start is called before the first frame update
    void Start()
    {
        scoreValue.text = "SCORE: " + ScoreCounter.totalScore;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
