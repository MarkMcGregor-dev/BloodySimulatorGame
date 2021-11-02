using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameObject HostController;

    private float radius;
    private int length;

    private Transform artery;

    [SerializeField] private GameObject redBloodCell;
    [SerializeField] private GameObject blockage;

    private void Start()
    {
        HostController = GameObject.Find("HostController");
        artery = transform;

        // Rough values relating to the artery itself:
        radius = 8;
        length = 20;

        SpawnBloodCells();

        if(BlockageCheck())
        {
            SpawnBlockage();
        }

    }

    private void SpawnBloodCells()
    {
        Transform artery = transform;
        float x;
        float y;
        float z;

        for (int i = 0; i < 3; i++)
        {
            // Check which way the artery is rotated in relation to world space:
            if(artery.rotation.eulerAngles.y == 0 || artery.rotation.eulerAngles.y == 180)
            {
                z = -((length - 2) / 3) + (((length - 2) / 3) * i);
                x = Random.Range(-radius + 1.0f, radius - 1.0f);
            }

            else
            {
                x = -((length - 2) / 3) + (((length - 2) / 3) * i);
                z = Random.Range(-radius + 1.0f, radius - 1.0f);
            }

            y = Random.Range(-radius + 1.0f, radius - 1.0f);

            Instantiate(redBloodCell, new Vector3(x, y, z) + artery.position, Quaternion.identity, artery);
        }
    }

    private void SpawnBlockage()
    {
        const float depth = 15.0f;

        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;

        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        float randomRotation = Random.Range(0.0f, 360.0f);

        if (artery.rotation.eulerAngles.y == 0 || artery.rotation.eulerAngles.y == 180)
        {
            z = depth;
            rotation = Quaternion.Euler(0.0f, 0.0f, randomRotation);
        }

        else
        {
            x = depth;
            rotation = Quaternion.Euler(randomRotation, 0.0f, 0.0f);

        }

        Instantiate(blockage, new Vector3(x, y, z) + artery.position, rotation, artery);
    }

    private bool BlockageCheck()
    {
        float minHeartRate = HostController.GetComponent<HostController>().minHeartRate;
        float idealHeartRate = HostController.GetComponent<HostController>().idealHeartRate;

        float currentHeartRate = HostController.GetComponent<HostController>().currentHeartRate;

        int random = Random.Range(1, 11);

        if(currentHeartRate < minHeartRate + 50.0f)
        {
            if(random <= 7)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        else if(currentHeartRate > idealHeartRate - 40.0f)
        {
            if (random <= 2)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        else
        {
            if (random <= 4)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
