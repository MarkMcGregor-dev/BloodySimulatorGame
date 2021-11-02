using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCellCollector : MonoBehaviour
{
    public GameObject bloodCellPrefab;

    private void OnEnable()
    {
        // like and subscribe
        PlayerController.PlayerCollectedBloodCell += OnCollectBloodCell;
    }

    private void OnDisable()
    {
        PlayerController.PlayerCollectedBloodCell -= OnCollectBloodCell;
    }

    void Start()
    {
        // setup variables
    }

    void Update()
    {
        
    }

    private void OnCollectBloodCell()
    {
        Debug.Log("COLLECTED");

        // create a new blood cell to follow the player
        GameObject Player = GameObject.Find("Player");

        if (Player.GetComponent<PlayerController>().numCellsCollected <= Player.GetComponent<PlayerController>().maxNumCells)
        {
            GameObject newBloodCell = Instantiate(bloodCellPrefab, transform.position, Quaternion.identity, transform);
        }
    }
}
