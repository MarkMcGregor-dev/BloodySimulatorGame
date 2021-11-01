using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
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
        transform.position = player.transform.position + startPos;
    }
}
