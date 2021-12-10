using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class SpawnerV2 : MonoBehaviour
{
    [Tooltip("The distance from the player this spawner should travel at along the spline")]
    public float distanceFromPlayer;

    [Header("Blood Cell Config")]
    public GameObject bloodCellPrefab;
    [Tooltip("How far the spawner should travel before triggering another blood cell spawn (lower = more spawning)")]
    public float cellSpawnFrequency;
    [Tooltip("The low number of cells to spawn at once (when energy < 20)")]
    public int lowCellCount;
    [Tooltip("The normal number of cells to spawn at once")]
    public int normalCellCount;
    [Tooltip("The high number of cells to spawn at once (when energy > 90)")]
    public int highCellCount;
    [Tooltip("The allowed radius from the center of the spline in which cells can be spawned")]
    public float cellSpawnRadius;

    [Header("Blockage Config")]
    public GameObject blockagePrefab;
    [Tooltip("How far the spawner should travel before triggering another blockage spawn (lower = more spawning)")]
    public float blockageSpawnFrequency;
    [Tooltip("The radius from the center of the spline in which blockages are spawned")]
    public float blockageSpawnRadius;

    [Header("Powerup Config")]
    public GameObject sandwichPrefab;
    public GameObject coffeePrefab;
    [Tooltip("How far the spawner should travel before triggering another blockage spawn (lower = more spawning)")]
    public float powerupSpawnFrequency;
    [Tooltip("The radius from the center of the spline in which blockages are spawned")]
    public float powerupSpawnRadius;

    private Spline levelSpline;
    private PlayerController playerController;
    private float lastPlayerDistance;
    private float distanceTravelled;
    private float distanceOfLastCellSpawn;
    private Transform cellSpawnParent;
    private HostController hostController;
    private float distanceOfLastBlockageSpawnAttempt;
    private Transform blockageSpawnParent;
    private Transform powerupSpawnParent;
    private float distanceOfLastPowerupSpawn;

    void Start()
    {
        // setup variables
        levelSpline = FindObjectOfType<Spline>();
        playerController = FindObjectOfType<PlayerController>();
        distanceTravelled = 0f;
        lastPlayerDistance = 0f;
        distanceOfLastCellSpawn = 0f;
        cellSpawnParent = transform.parent.Find("BloodCells");
        hostController = FindObjectOfType<HostController>();
        distanceOfLastBlockageSpawnAttempt = 0f;
        blockageSpawnParent = transform.parent.Find("Blockages");
        powerupSpawnParent = transform.parent.Find("Powerups");
        distanceOfLastPowerupSpawn = 0f;
    }

    void Update()
    {
        // update the position of the spawner on the spline
        UpdatePosition();

        // check if enough distance has passed since the last spawn to trigger a spawn
        if ((distanceTravelled - distanceOfLastCellSpawn) >= cellSpawnFrequency)
        {
            SpawnCellCluster();
        }

        // check if a blockage should be spawned
        if (ShouldSpawnBlockage())
        {
            SpawnBlockage();
        }

        // check if enough distance has passed since the last spawn to trigger a spawn
        if ((distanceTravelled - distanceOfLastPowerupSpawn) >= powerupSpawnFrequency)
        {
            SpawnPowerup();
        }
    }

    private void UpdatePosition()
    {
        // get the distance of the player along the spline
        float playerDistanceAlongSpline = playerController.distanceInLevel;

        // calculate the difference in distance travelled and add it to the total
        distanceTravelled += Mathf.Max(0, playerDistanceAlongSpline - lastPlayerDistance);
        lastPlayerDistance = playerDistanceAlongSpline;

        // get a curve sample at the desired distance from the player
        CurveSample curveSample = levelSpline.GetSampleAtDistance(playerDistanceAlongSpline + distanceFromPlayer);

        // set the transform of the spawner to the curve sample location and rotation
        transform.SetPositionAndRotation(curveSample.location, curveSample.Rotation);
    }

    private void SpawnCellCluster()
    {
        // get the current energy from the host controller
        float currentEnergy = hostController.currentEnergy;

        // determine how many cells should be spawned
        //int numOfCells = Random.Range(0, maxSpawnAmount);
        int numOfCells = currentEnergy < 180 ? highCellCount : normalCellCount;

        // spawn the desired number of cells
        for (int i = 0; i < numOfCells; i++)
        {
            SpawnCell();
        }

        // update the distance of the last spawn
        distanceOfLastCellSpawn = distanceTravelled;
    }

    private void SpawnCell()
    {
        // get a random direction from the center of the spline to spawn the cell at
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        // get a random distance from the center
        float distanceFromCenter = Random.Range(0f, cellSpawnRadius);

        // get the location of the cell to spawn
        Vector3 spawnLocation = transform.position + ((transform.right * direction.x + transform.up* direction.y) * distanceFromCenter);

        // spawn a new bloodcell at the location with the correct parent
        Instantiate<GameObject>(bloodCellPrefab, spawnLocation, Random.rotation, cellSpawnParent);
    }

    private void SpawnBlockage()
    {
        // get a random rotation for the blockage to be spawned at
        float rotationDegree = Random.Range(0f, 360f);

        Vector3 spawnLocation = transform.position + (transform.up * blockageSpawnRadius);

        // create the new blockage
        GameObject newBlockage = Instantiate(blockagePrefab, spawnLocation, transform.rotation, blockageSpawnParent);

        // rotate the blockage according to the random rotation
        newBlockage.transform.RotateAround(transform.position, transform.forward, rotationDegree);
    }

    private void SpawnPowerup()
    {
        // Get a random direction from the center of the spline to spawn the powerup at:
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        // Get a random distance from the center:
        float distanceFromCenter = Random.Range(0f, powerupSpawnRadius);

        // Get the location of the cell to spawn
        Vector3 spawnLocation = transform.position + ((transform.right * direction.x + transform.up * direction.y) * distanceFromCenter);

        // calculate the high-end probability of a powerup spawning accounting for the current energy level
        // (this will cause powerups to spawn more when the heartrate is low)
        float powerupSpawnMaxProb = Mathf.Lerp(
            1.5f, 8f, Mathf.InverseLerp(hostController.minHeartRate, hostController.idealHeartRate, hostController.currentHeartRate));

        float random = Random.Range(0f, powerupSpawnMaxProb);
        GameObject powerup;


        if (random <= 1f) {
            powerup = sandwichPrefab;
        } else if (random <= 2f)
        {
            powerup = coffeePrefab;
        } else
        {
            powerup = null;
        }

        //switch (random)
        //{
        //    case 1:
        //        powerup = sandwichPrefab;
        //        break;

        //    case 2:
        //        powerup = coffeePrefab;
        //        break;

        //    default:
        //        powerup = null;
        //        break;
        //}

        if (powerup != null)
        {
            Instantiate<GameObject>(powerup, spawnLocation, Quaternion.identity, powerupSpawnParent);
        }

        // update the distance of the last spawn
        distanceOfLastPowerupSpawn = distanceTravelled;
    }

    private bool ShouldSpawnBlockage()
    {
        // check if enough distance has been travelled to spawn another blockage
        if ((distanceTravelled - distanceOfLastBlockageSpawnAttempt) >= blockageSpawnFrequency)
        {
            // get the heartrate values from the host controller
            float minHeartRate = hostController.minHeartRate;
            float idealHeartRate = hostController.idealHeartRate;
            float currentHeartRate = hostController.currentHeartRate;

            int random = Random.Range(1, 101);
            Debug.Log(random);

            distanceOfLastBlockageSpawnAttempt = distanceTravelled;

            if (currentHeartRate < minHeartRate + 50f)
            {
                return (random <= 95);
            }

            else if (currentHeartRate > idealHeartRate - 40f)
            {
                return (random <= 30);
            }

            else
            {
                return (random <= 80);
            }

            
        }

        else
        {
            return false;
        }
    }
}