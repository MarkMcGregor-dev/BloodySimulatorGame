using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    private GameObject HostController;

    float minHeartRate;
    float idealHeartRate;
    float currentHeartRate;
    float currentEnergy;

    private float radius;
    private int length;

    private GameObject sandwichButton;

    private Transform artery;

    [SerializeField] private GameObject redBloodCell;
    [SerializeField] private GameObject blockage;

    private void Start()
    {
        HostController = GameObject.Find("HostController");
        sandwichButton = GameObject.Find("Sandwich");

        minHeartRate = HostController.GetComponent<HostController>().minHeartRate;
        idealHeartRate = HostController.GetComponent<HostController>().idealHeartRate;
        currentHeartRate = HostController.GetComponent<HostController>().currentHeartRate;

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

        int numBloodCells = 2;

        currentEnergy = HostController.GetComponent<HostController>().currentEnergy;

        if (currentEnergy < 20.0f)
        {
            numBloodCells = 0;
        }

        else if (currentHeartRate > 90.0f)
        {
            numBloodCells = 2;
        }

        else
        {
            numBloodCells = 1;
        }

        if (sandwichButton.GetComponent<SandwichButton>().sandwichActivated == true)
        {
            numBloodCells = 3;
        }

        for (int i = 0; i < numBloodCells; i++)
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
        minHeartRate = HostController.GetComponent<HostController>().minHeartRate;
        idealHeartRate = HostController.GetComponent<HostController>().idealHeartRate;
        currentHeartRate = HostController.GetComponent<HostController>().currentHeartRate;

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
