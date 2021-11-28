using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;
using UnityEngine.UI;

public class RiderTest : MonoBehaviour
{
    public float movementSpeed;

    private Spline spline;
    private float distanceTravelled;

    void Start()
    {
        // setup variables
        spline = FindObjectOfType<Spline>();
        distanceTravelled = 0f;
    }

    void Update()
    {
        // update distanceTravelled
        distanceTravelled = distanceTravelled + (movementSpeed * Time.deltaTime);

        // transform.position = new Vector3(0f, 0f, distanceTravelled);

        // calculate a distance vector to be used for sampling the curve
        Vector3 distanceVector = new Vector3(0f, 0f, distanceTravelled);

        // translate along the spline by distance travelled
        //CurveSample curveSample = spline.GetProjectionSample(distanceVector);
        CurveSample curveSample = spline.GetSampleAtDistance(distanceTravelled);
        transform.SetPositionAndRotation(curveSample.location, curveSample.Rotation);
    }
}
