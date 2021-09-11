using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] GameObject EndLevelCanvasPrefab;
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
        SendStatusToGameManager(lvlStatus);

    }

    void SendStatusToGameManager(LevelCompletionStatus status)
    {
        GameManager.Instance.LevelEnded(status);
        Instantiate(EndLevelCanvasPrefab).GetComponent<EndLevelCanvas>().FinishedLevel(status);
    }
}
