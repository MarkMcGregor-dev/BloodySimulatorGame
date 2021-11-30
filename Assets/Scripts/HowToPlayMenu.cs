using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayMenu : MonoBehaviour
{
    public GameObject rules;
    public GameObject controls;
    public GameObject blockageTutorial;
    public GameObject heartrateTutorial;
    public GameObject nextButton;
   
    void Start()
    {
        rules.SetActive(true);
        controls.SetActive(true);

        blockageTutorial.SetActive(false);
        heartrateTutorial.SetActive(false);
    }

    public void NextPressed()
    {
        rules.SetActive(false);
        controls.SetActive(false);

        blockageTutorial.SetActive(true);
        heartrateTutorial.SetActive(true);
        nextButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
