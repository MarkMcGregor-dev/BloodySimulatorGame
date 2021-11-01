using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HostDeathReason
{
    HeartRateHigh,
    HeartRateLow,
    EnergyDepleted
}

public enum HostState
{
    Dead,
    Unconcious,
    Alive
}

public class HostController : MonoBehaviour
{
    // events
    public delegate void HeartRateDelegate(float newHeartRate);
    public static event HeartRateDelegate HeartRateChanged;

    public delegate void EnergyDelegate(float newEnergy);
    public static event EnergyDelegate EnergyChanged;

    public delegate void HostDeathDelegate(HostDeathReason deathReason);
    public static event HostDeathDelegate HostDied;

    // config variables
    [Header("Config")]
    public float minHeartRate   = 30f;
    public float maxHeartRate   = 220f;
    public float maxEnergy      = 100f;
    public float minEnergy      = 0f;
    public float energyDepletionScaler = 1f;
    public float heartRateDeltaScaler = 1f;
    [Tooltip("The frequency at which the host is simulated (in seconds)")]
    public float updateRate;

    // script variables
    private HostState hostState;
    [Header("Do not edit")]
    public float currentHeartRate;
    public float desiredHeartRate;
    public float currentEnergy;
    private float timeOfLastStep;

    private void OnEnable()
    {
        // setup event listeners
        GameController.GameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        // clear event listeners
        GameController.GameStarted -= OnGameStarted;
    }

    void Start()
    {
        // setup variables
        currentHeartRate = (minHeartRate + maxHeartRate) / 2f; // heartrate starts at the average
        currentEnergy = (maxEnergy - minEnergy) * 1f;
        hostState = HostState.Dead;
        timeOfLastStep = Time.time;
    }

    void Update()
    {
        // make sure the host isn't dead
        if (hostState != HostState.Dead)
        {
            // check if the heartrate is too low
            if (currentHeartRate < minHeartRate)
            {
                // host dies
                if (HostDied != null) HostDied(HostDeathReason.HeartRateLow);
                hostState = HostState.Dead;

            // check if the heartrate is too high
            } else if (currentHeartRate > maxHeartRate)
            {
                // host dies
                if (HostDied != null) HostDied(HostDeathReason.HeartRateHigh);
                hostState = HostState.Dead;

            } else
            {
                float timeSinceLastStep = Time.time - timeOfLastStep;

                // check if it is time to simulate a step
                if (timeSinceLastStep >= updateRate)
                {
                    // calculate the adjustment to the energy
                    float nextEnergyVal = currentEnergy - (currentHeartRate * energyDepletionScaler * timeSinceLastStep);
                    currentEnergy = Mathf.Clamp(nextEnergyVal, minEnergy, maxEnergy);

                    // calculate the adjustment to the heartrate
                    float energyInfluence = currentEnergy / (maxEnergy - minEnergy); // 0 to 1
                    desiredHeartRate = Mathf.Lerp(minHeartRate, (minHeartRate + maxHeartRate) / 2f, energyInfluence);
                    currentHeartRate = Mathf.Lerp(currentHeartRate, desiredHeartRate, heartRateDeltaScaler * Time.deltaTime);

                    // fire events
                    if (EnergyChanged != null) EnergyChanged(currentEnergy);
                    if (HeartRateChanged != null) HeartRateChanged(currentHeartRate);

                    timeOfLastStep = Time.time;
                }
            }
        }
    }

    private void OnGameStarted()
    {
        // reset variables
        currentHeartRate = (minHeartRate + maxHeartRate) / 2f; // heartrate starts at the average
        currentEnergy = (maxEnergy - minEnergy) * 1f;
        hostState = HostState.Alive;
        timeOfLastStep = Time.time;
    }
}
