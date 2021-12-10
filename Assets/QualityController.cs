using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityController : MonoBehaviour
{
    // frameRates to determine to switch settings
    public double maxFrameRate;
    public double minFrameRate;
    
    // threshold to ignore frames dropped
    public int switchThreshold;

    int upCounter;
    int downCounter;

    void Update()
    {
        if ((1 / Time.smoothDeltaTime) > maxFrameRate)
        {
            upCounter++;
            if (upCounter > switchThreshold)
            {
                QualitySettings.IncreaseLevel();
                upCounter = -3;
            }
        }
        else
        {
            upCounter = 0;
        }
        //Debug.Log(upCounter);

        if ((1 / Time.smoothDeltaTime) < minFrameRate)
        {
            downCounter++;
            if (downCounter > switchThreshold)
            {
                QualitySettings.DecreaseLevel();
                downCounter = -3;
            }
        }
        else
        {
            downCounter = 0;
        }
    }
}
