using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    private LevelCompletionStatus lvlStatus;

    private void Start()
    {
        lvlStatus = new LevelCompletionStatus();
    }

    public void CaughtByBoss()
    {
        lvlStatus.timesBossDetected++;
    }

    public void SetFinalStress(float s)
    {
        lvlStatus.finalStress = s;
    }

    public void FinishedLevel()
    {

    }
}
