using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockageBehaviour : MonoBehaviour
{
    public delegate void BlockageBreakDelegate();
    public static event BlockageBreakDelegate BlockageBroken;

    public Color breakableColor;
    public float breakableColorEmission;

    private GameObject player;
    private Material blockageMat;
    private Color c;
    private float a = 0.0f;
    private bool goingUp;

    private Color transparent = new Color(27/255f, 229/255f, 108/255f, 0.5f);

    private int cellsToBreakBlockage;

    private void Start()
    {
        player = GameObject.Find("Player");

        blockageMat = transform.GetComponent<MeshRenderer>().material;

        c = blockageMat.GetColor("Color_8a2c3fc848354067aa332bfb7bd854fc");
        goingUp = true;

        cellsToBreakBlockage = player.GetComponent<PlayerController>().cellsToBreakBlockage;
    }

    private void Update()
    {
        if (player.GetComponent<PlayerController>().numCellsCollected >= cellsToBreakBlockage)
        {
            c.a = goingUp ? .005f + c.a : c.a - .005f;

            if (c.a >= 0.5f)
                goingUp = false;
            else if (c.a <= 0.0f)
                goingUp = true;

           blockageMat.SetColor("Color_8a2c3fc848354067aa332bfb7bd854fc", breakableColor * breakableColorEmission);
        }

        else
        {
            blockageMat.SetColor("Color_8a2c3fc848354067aa332bfb7bd854fc", c);
        }
        
    }

    public void BreakBlockage()
    {
        // fire event
        if (BlockageBroken != null) BlockageBroken();

        // destroy the object
        Destroy(gameObject);

        GameObject CollectedBloodCells = player.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;

        for (int i = 0; i < cellsToBreakBlockage; i++)
        {
            Destroy(CollectedBloodCells.transform.GetChild(i).gameObject);
        }
    }
}