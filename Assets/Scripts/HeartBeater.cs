using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartBeater : MonoBehaviour
{
    public AnimationCurve beatCurve;

    private HostController hostController;
    private Image heartImage;

    void Start()
    {
        // setup variables
        hostController = FindObjectOfType<HostController>();
        heartImage = GetComponent<Image>();
    }

    void Update()
    {
        float rate = Mathf.InverseLerp(
            hostController.idealHeartRate, hostController.minHeartRate, hostController.currentHeartRate);

        float curveVal = beatCurve.Evaluate(Time.time % Mathf.Max(0.1f, Mathf.Lerp(0.5f, 1.5f, rate)));

        heartImage.transform.localScale = new Vector3(curveVal, curveVal, 1);
    }
}
