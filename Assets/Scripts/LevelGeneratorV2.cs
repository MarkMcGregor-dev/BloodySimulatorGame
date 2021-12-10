using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class LevelGeneratorV2 : MonoBehaviour
{
    public delegate void LevelSectionRemovedDelegate(float curveLength);
    public static event LevelSectionRemovedDelegate LevelSectionRemoved;

    public int numberOfDesiredSections;
    public float gapBetweenPoints;
    public float spawnRadius;
    public Vector3 pointDirection;
    public bool enableCleanup;

    [Range(0f, 1f)]
    public float precentPastPointToCleanup;

    private Vector3 lastPointPos;
    private Spline spline;
    private PlayerController player;

    void Start()
    {
        // setup variables
        spline = GetComponent<Spline>();
        player = FindObjectOfType<PlayerController>();

        // move the second spline point to a better position
        lastPointPos = spline.nodes[0].Position;
        Vector3 newP2Position = GetNewPoint();
        spline.nodes[1].Position = newP2Position;
        spline.nodes[1].Direction = newP2Position + pointDirection;
        lastPointPos = spline.nodes[1].Position;

        // generate the starting sections
        for (int i = 0; i < (numberOfDesiredSections - 1); i++)
        {
            AddSectionToSpline();
        }
    }

    void Update()
    {
        // get the location of the early points in the spline
        Vector3 pLocation = spline.nodes[1].Position;
        
        // check if the player is past the third point
        if (player.transform.position.z > (pLocation.z + (gapBetweenPoints * precentPastPointToCleanup)))
        {
            if (enableCleanup)
            {
                // delete a section from the spline
                RemoveSectionFromSpline();
            }

            // generate a new section
            AddSectionToSpline();
        }
    }

    private Vector3 GetNewPoint()
    {
        // get random values within the spawnRadius for x and y
        float xPos = Random.Range(-spawnRadius, spawnRadius);
        float yPos = Random.Range(-spawnRadius, spawnRadius);

        // create the new point considering the lastPointPos and the gapBetweenPoints
        Vector3 newPoint = new Vector3(xPos, yPos, lastPointPos.z + gapBetweenPoints);

        return newPoint;
    }

    private void AddSectionToSpline()
    {
        // get a new point for the curve and update the lastPointPos
        Vector3 newPoint = GetNewPoint();
        lastPointPos = newPoint;

        // create a new node and add it to the spline
        spline.AddNode(new SplineNode(newPoint, newPoint + pointDirection));
    }

    private void RemoveSectionFromSpline()
    {
        // get the length of the first curve in the spline
        float curveLength = spline.curves[0].Length;

        // remove the first node in the spline
        spline.RemoveNode(spline.nodes[0]);

        // call the section removed event
        if (LevelSectionRemoved != null) LevelSectionRemoved(curveLength);

        Debug.Log("Removed section from spline");
    }
}
