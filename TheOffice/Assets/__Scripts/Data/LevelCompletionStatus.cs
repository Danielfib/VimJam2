using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletionStatus
{
    public float finalStress;
    public int timesBossDetected;
    //1 - Stress always under 70% | 2- Boss never caught | 3- Completed the long task
    public bool hasFirstStar, hasSecondStar, hasThirdStar;

    public LevelCompletionStatus()
    {
        finalStress = 0;
        timesBossDetected = 0;
        hasFirstStar = true;
        hasSecondStar = true;
        hasThirdStar = false;
    }

    public void LoseFirstStar() { hasFirstStar = false; }
    public void LoseSecondStar() { hasSecondStar = false; }
    public void LoseThirdStar() { hasThirdStar = false; }
}
