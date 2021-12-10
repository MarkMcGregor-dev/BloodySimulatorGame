using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    public float effectBeginValue;
    public AnimationCurve effectCurve;
    public bool enableSaturation;
    [Range(-100f, 0f)]
    public float minSaturation;

    private HostController hostController;
    private Volume ppVolume;
    private Vignette vignette;
    private ColorAdjustments colorAdjustments;

    void Start()
    {
        // setup variables
        ppVolume = GetComponent<Volume>();
        hostController = FindObjectOfType<HostController>();

        Vignette tmpV;
        ColorAdjustments tmpC;

        if (ppVolume.profile.TryGet<Vignette>(out tmpV))
        {
            vignette = tmpV;
        }

        if (ppVolume.profile.TryGet<ColorAdjustments>(out tmpC))
        {
            colorAdjustments = tmpC;
        }
    }

    void Update()
    {
        float deathValue = Mathf.InverseLerp(
            hostController.minHeartRate, hostController.idealHeartRate, hostController.currentHeartRate);

        float colorValue = effectCurve.Evaluate(deathValue);

        if (enableSaturation)
        {
            float newColorValue = Mathf.Lerp(minSaturation, 0f, colorValue);
            colorAdjustments.saturation.value = newColorValue;
        }

        vignette.intensity.value = Mathf.Lerp(0.75f, 0.25f, colorValue);
    }
}
