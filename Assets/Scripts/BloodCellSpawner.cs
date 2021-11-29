using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class BloodCellSpawner : MonoBehaviour
{
    public GameObject bloodCellPrefab;
    [Tooltip("The distance from the player this spawner should travel at along the spline")]
    public float distanceFromPlayer;
    [Tooltip("How far the spawner should travel before triggering another spawn (lower = more spawning)")]
    public float spawnFrequency;
    [Tooltip("The maximum number of cells to spawn at once")]
    public int maxSpawnAmount;
    [Tooltip("The radius from the center of the spline in which cells can be spawned")]
    public float spawnRadius;

    private Spline levelSpline;
    private PlayerController playerController;
    public float lastPlayerDistance;
    public float distanceTravelled;
    public float distanceOfLastSpawn;
    private Transform spawnParent;

    void Start()
    {
        // setup variables
        levelSpline = FindObjectOfType<Spline>();
        playerController = FindObjectOfType<PlayerController>();
        distanceTravelled = 0f;
        lastPlayerDistance = 0f;
        distanceOfLastSpawn = 0f;
        spawnParent = transform.parent.Find("BloodCells");
    }

    void Update()
    {
        // update the position of the spawner on the spline
        UpdatePosition();

        // check if enough distance has passed since the last spawn to trigger a spawn
        if ((distanceTravelled - distanceOfLastSpawn) >= spawnFrequency)
        {
            SpawnCellCluster();
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
        // determine how many cells should be spawned
        int numOfCells = Random.Range(0, maxSpawnAmount);

        // spawn the desired number of cells
        for (int i = 0; i < numOfCells; i++)
        {
            SpawnCell();
        }

        // update the distance of the last spawn
        distanceOfLastSpawn = distanceTravelled;
    }

    private void SpawnCell()
    {
        // get a random direction from the center of the spline to spawn the cell at
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        // get a random distance from the center
        float distanceFromCenter = Random.Range(0f, spawnRadius);

        // get the location of the cell to spawn
        Vector3 spawnLocation = transform.position + ((transform.right * direction.x + transform.up* direction.y) * distanceFromCenter);

        // spawn a new bloodcell at the location with the correct parent
        Instantiate<GameObject>(bloodCellPrefab, spawnLocation, Random.rotation, spawnParent);
    }
}
