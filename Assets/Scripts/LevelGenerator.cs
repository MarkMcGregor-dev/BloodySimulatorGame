using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject arteryPiece;

    public int numPiecesBuffer = 5; // the number of pieces that should exist at all times
    private const float distanceBetweenPieces = 20;

    private Transform targetParent;
    private int numOfPiecesSpawned;
    private float lastZOffset; // the z-position of the last piece that was spawned

    private void OnEnable()
    {
        // subscribe to events
        DeleterBehaviour.DeleterActivated += OnPieceDeleted;
    }

    private void OnDisable()
    {
        // unsubscribe from events
        DeleterBehaviour.DeleterActivated -= OnPieceDeleted;
    }

    void Start()
    {
        // setup variables
        numOfPiecesSpawned = 1;
        lastZOffset = 0;
        targetParent = GameObject.Find("ArteryPieces").transform;

        // spawn initial pieces
        while (numOfPiecesSpawned < numPiecesBuffer)
        {
            SpawnArteryPieces();
        }
    }

    void Update()
    {
        SpawnArteryPieces();
    }

    private void SpawnArteryPieces()
    {
        // check if new pieces need to be spawned
        if (numOfPiecesSpawned < numPiecesBuffer)
        {
            // calculate the zOffset of the next piece
            float zOffset = lastZOffset + distanceBetweenPieces;

            // spawn a new artery piece
            SpawnPiece(zOffset);

            // update spawning variables
            lastZOffset = zOffset;
            numOfPiecesSpawned++;
        }
    }

    private void SpawnPiece(float zOffset)
    {
        // instantiate the new artery piece
        Instantiate(arteryPiece, new Vector3(0, 0, zOffset), Quaternion.identity, targetParent);
    }

    private void OnPieceDeleted()
    {
        // decrement the number of pieces spawned
        numOfPiecesSpawned -= 1;
    }
}
