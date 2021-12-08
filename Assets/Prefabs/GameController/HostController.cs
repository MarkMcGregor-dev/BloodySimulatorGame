using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HostDeathReason
{
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
    public float idealHeartRate = 220f;
    public float maxEnergy      = 100f;
    public float minEnergy      = 0f;
    public float energyDepletionScaler = 1f;
    public float heartRateDeltaScaler = 1f;
    public float energyFromBloodCells = 5f;
    public float blockageHeartRateIncrement = 20f;
    public float coffeeHeartRateIncrement= 40f;
    [Tooltip("The frequency at which the host is simulated (in seconds)")]
    public float updateRate;

    // script variables
    [Header("Do not edit")]
    public float currentHeartRate;
    public float desiredHeartRate;
    public float currentEnergy;
    private HostState hostState;
    private float timeOfLastStep;

    private void OnEnable()
    {
        // setup event listeners
        GameController.GameStarted += OnGameStarted;
        PlayerController.PlayerCollectedBloodCell += OnCollectBloodCell;
        BlockageBehaviour.BlockageBroken += OnBlockageBroken;
    }

    private void OnDisable()
    {
        // clear event listeners
        GameController.GameStarted -= OnGameStarted;
        PlayerController.PlayerCollectedBloodCell -= OnCollectBloodCell;
        BlockageBehaviour.BlockageBroken -= OnBlockageBroken;
    }

    void Start()
    {
        // setup variables
        currentHeartRate = idealHeartRate; // heartrate starts at the ideal value
        desiredHeartRate = idealHeartRate;
        currentEnergy = (maxEnergy - minEnergy) * 1f;
        hostState = HostState.Alive;
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
                UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreen", UnityEngine.SceneManagement.LoadSceneMode.Single);

            }
            else
            {
                float timeSinceLastStep = Time.time - timeOfLastStep;

                // check if it is time to simulate a step
                if (timeSinceLastStep >= updateRate)
                {
                    // calculate the adjustment to the heartrate
                    currentHeartRate = currentHeartRate - (heartRateDeltaScaler * timeSinceLastStep);

                    // calculate the adjustment to the energy
                    currentEnergy = currentEnergy - (Mathf.Min(currentHeartRate, idealHeartRate/2)
                        * energyDepletionScaler * timeSinceLastStep);

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
        currentHeartRate = idealHeartRate; // heartrate starts at the ideal value
        desiredHeartRate = idealHeartRate;
        currentEnergy = (maxEnergy - minEnergy) * 1f;
        hostState = HostState.Alive;
        timeOfLastStep = Time.time;
    }

    private void OnCollectBloodCell()
    {
        currentEnergy = Mathf.Clamp(currentEnergy + energyFromBloodCells, minEnergy, maxEnergy);
    }

    private void OnBlockageBroken()
    {
        currentHeartRate = Mathf.Min(currentHeartRate + blockageHeartRateIncrement, desiredHeartRate);
        OnHeartRateIncrease(blockageHeartRateIncrement.ToString());

    }

    public void OnCoffeeDrink()
    {
        currentHeartRate = Mathf.Min(currentHeartRate + coffeeHeartRateIncrement, desiredHeartRate);
        OnHeartRateIncrease(coffeeHeartRateIncrement.ToString());

    }

    private void OnHeartRateIncrease(string value)
    {
        Animation anim;

        var plusText = GameObject.Find("Plus Text");

        plusText.GetComponent<Text>().text = "+" + value;

        anim = plusText.GetComponent<Animation>();
        anim.Stop();
        anim.Play();

        var bpm = GameObject.Find("Heart Panel");

        for (int i = 1; i < 4; i++)
        {
            anim = bpm.transform.GetChild(i).GetComponent<Animation>();
            anim.Stop();
            anim.Play();
        }
    }
}
