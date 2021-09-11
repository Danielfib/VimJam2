using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletionStatus
{
    public float finalStress;
    public int timesBossDetected;

    public LevelCompletionStatus()
    {
        finalStress = 0;
        timesBossDetected = 0;
    }
}
