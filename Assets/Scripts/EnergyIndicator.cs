using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyIndicator : MonoBehaviour
{
    private HostController hostController;
    private Slider slider;

    void Start()
    {
        // setup variables
        hostController = FindObjectOfType<HostController>();
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        var heartRate = hostController.currentHeartRate;
        float fillValue;

        if(heartRate > 175)
        {
            fillValue = 1f;
        }

        else if(heartRate < 75)
        {
            fillValue = 0.333f;
        }

        else
        {
            fillValue = 0.666f;
        }

        // update the slider
        slider.value = fillValue;
    }
}
