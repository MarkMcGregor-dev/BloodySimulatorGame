using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockageBehaviour : MonoBehaviour
{
    public delegate void BlockageBreakDelegate();
    public static event BlockageBreakDelegate BlockageBroken;

    public Color breakableColor;
    public float breakableColorEmission;

    public GameObject blockageEffect;

    private GameObject player;
    private Material blockageMat;
    private Color c;

    private int cellsToBreakBlockage;

    private void Start()
    {
        player = GameObject.Find("Player");

        blockageMat = transform.GetComponent<MeshRenderer>().material;

        c = blockageMat.GetColor("Color_8a2c3fc848354067aa332bfb7bd854fc");

        cellsToBreakBlockage = player.GetComponent<PlayerController>().cellsToBreakBlockage;
    }

    private void Update()
    {
        if (player.GetComponent<PlayerController>().numCellsCollected >= cellsToBreakBlockage)
        {
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

        // Spawn a particle system relative to the player:
        var obj = Instantiate(blockageEffect, player.transform, false);

        // Translate it ahead of the player:
        obj.transform.localPosition = new Vector3(0, 0, 5);

        // Unparent, play the particle effect, and then destroy the object
        obj.transform.parent = null;
        obj.GetComponent<ParticleSystem>().Play();
        Destroy(obj, 3f);

        // Destroy this Blockage: 
        Destroy(gameObject);

        GameObject CollectedBloodCells = player.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;

        for (int i = 0; i < cellsToBreakBlockage; i++)
        {
            Destroy(CollectedBloodCells.transform.GetChild(i).gameObject);
        }
    }
}