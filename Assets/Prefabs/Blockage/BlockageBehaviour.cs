using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockageBehaviour : MonoBehaviour
{
    public delegate void BlockageBreakDelegate();
    public static event BlockageBreakDelegate BlockageBroken;

    private GameObject player;

    private Material blockageMat;
    private Color c;
    private float a = 0.0f;
    private bool goingUp;

    private Color transparent = new Color(27, 229, 108, 0);

    private void Start()
    {
        player = GameObject.Find("Player");

        blockageMat = transform.GetChild(0).GetComponent<MeshRenderer>().material;

        c = blockageMat.GetColor("_BaseColor");
        goingUp = true;
    }

    private void Update()
    {
        if (player.GetComponent<PlayerController>().numCellsCollected >= 5)
        {
            c.a = goingUp ? .005f + c.a : c.a - .005f;

            if (c.a >= 0.5f)
                goingUp = false;
            else if (c.a <= 0.0f)
                goingUp = true;

           blockageMat.SetColor("_BaseColor", c);
        }

        else
        {
            blockageMat.SetColor("_BaseColor", transparent);
        }
        
    }

    public void BreakBlockage()
    {
        // fire event
        if (BlockageBroken != null) BlockageBroken();

        // destroy the object
        Destroy(gameObject);

        GameObject CollectedBloodCells = player.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;

        for (int i = 0; i < 5; i++)
        {
            Destroy(CollectedBloodCells.transform.GetChild(i).gameObject);
        }
    }
}