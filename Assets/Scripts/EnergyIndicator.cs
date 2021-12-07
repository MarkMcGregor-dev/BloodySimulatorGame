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
        // calculate the fill value from the host controller's energy
        float fillValue = Mathf.InverseLerp(hostController.minEnergy, hostController.maxEnergy, hostController.currentEnergy);

        // update the slider
        slider.value = fillValue;
    }
}
