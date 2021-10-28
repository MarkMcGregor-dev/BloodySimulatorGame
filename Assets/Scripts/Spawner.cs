using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    private float radius;
    private int length;

    [SerializeField] private GameObject redBloodCell;

    void Awake()
    {
        radius = 5;
        length = 20;

        Spawn();
    }

    private void Spawn()
    {
        Transform artery = this.gameObject.transform;
        float x = 0;
        float y;
        float z = 0;

        // Spawn first blood cell:
        for (int i = 0; i < 3; i++)
        {
            if(artery.rotation.eulerAngles.y == 0 || artery.rotation.eulerAngles.y == 180)
            {
                z = -((length - 2) / 3) + ((length / 3) * i);
                x = Random.Range(-radius + 1.0f, radius - 1.0f);
            }

            else
            {
                x = -((length - 2) / 3) + ((length / 3) * i);
                z = Random.Range(-radius + 1.0f, radius - 1.0f);
            }

            y = Random.Range(-radius + 1.0f, radius - 1.0f);

            Instantiate(redBloodCell, new Vector3(x, y, z), Quaternion.identity, this.gameObject.transform);
        }
    }
}
