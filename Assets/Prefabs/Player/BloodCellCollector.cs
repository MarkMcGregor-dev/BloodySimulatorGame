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
        Instantiate(bloodCellPrefab, new Vector3(), Quaternion.identity, transform);
    }
}
