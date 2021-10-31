using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    public float rotationSpeed;

    void Start()
    {
        // randomize the initial rotation
        transform.rotation = Random.rotation;
    }

    void Update()
    {
        transform.Rotate(new Vector3(1, 1, 1), rotationSpeed * Time.deltaTime);
    }
}
