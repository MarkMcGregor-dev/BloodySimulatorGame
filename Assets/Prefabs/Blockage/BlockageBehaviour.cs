using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockageBehaviour : MonoBehaviour
{
    public delegate void BlockageBreakDelegate();
    public static event BlockageBreakDelegate BlockageBroken;

    public void BreakBlockage()
    {
        // fire event
        if (BlockageBroken != null) BlockageBroken();

        // destroy the object
        Destroy(gameObject);

        GameObject Player = GameObject.Find("Player");
        GameObject CollectedBloodCells = Player.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;

        for (int i = 0; i < 5; i++)
        {
            Destroy(CollectedBloodCells.transform.GetChild(i).gameObject);
        }
    }
}
