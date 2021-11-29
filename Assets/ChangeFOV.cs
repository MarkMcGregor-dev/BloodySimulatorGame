using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFOV : MonoBehaviour
{
    private HostController hostController;
    private float minHeartRate;
    private float idealHeartRate;

    private Camera cam;
    private float minFOV;
    private float maxFOV;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        hostController = FindObjectOfType<HostController>();
        idealHeartRate = hostController.idealHeartRate;
        minHeartRate = hostController.minHeartRate;

        minFOV = 35;
        maxFOV = 85;
    }

    // Update is called once per frame
    void Update()
    {
        float currentHeartRate = hostController.currentHeartRate;

        float mappedFOV;

        mappedFOV = (currentHeartRate - minHeartRate) / (idealHeartRate - minHeartRate) * (maxFOV - minFOV) + minFOV;

        cam.fieldOfView = mappedFOV;
    }
}
