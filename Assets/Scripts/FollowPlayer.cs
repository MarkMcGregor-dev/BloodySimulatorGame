using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public bool followRotation;
    public bool useRotationLerp;
    public float rotationLerpStrength;

    public bool usePositionLerp;
    public float positionLerpStrength;

    private GameObject player;
    private Vector3 startPos;

    void Start()
    {
        // setup variables
        player = GameObject.Find("Player");
        startPos = transform.position;
    }

    void Update()
    {
        // determine the new position of the object (depending on if using lerping or not)
        Vector3 newPosition = usePositionLerp
            ? Vector3.Lerp(transform.position, player.transform.position + startPos, positionLerpStrength * Time.deltaTime)
            : player.transform.position + startPos;

        transform.position = newPosition;

        if (followRotation)
        {
            // determine the new rotation of the object (depending on if using lerping or not)
            Quaternion newRotation = useRotationLerp
                ? Quaternion.RotateTowards(transform.rotation, player.transform.rotation, rotationLerpStrength * Time.deltaTime)
                : player.transform.rotation;

            transform.rotation = newRotation;
        }
    }
}
