using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] GameObject EndLevelCanvasPrefab;
    [SerializeField] RectTransform lostScreen;
    private LevelCompletionStatus lvlStatus;

    bool isStillPlaying = true;

    private void Start()
    {
        lvlStatus = new LevelCompletionStatus();
    }

    public void CaughtByBoss()
    {
        lvlStatus.timesBossDetected++;
        LoseSecondStar();
    }

    public void SetFinalStress(float s)
    {
        lvlStatus.finalStress = s;
    }

    public void FinishedLevel()
    {
        if (isStillPlaying)
        {
            SFXManager.Instance.Win();
            isStillPlaying = false;
            StopLevel();
            SendStatusToGameManager(lvlStatus);
        }
    }

    public void Lost()
    {
        if (isStillPlaying)
        {
            isStillPlaying = false;
            Invoke("OpenLostScreen", 1f);
        }
    }

    void OpenLostScreen()
    {
        StopLevel();
        SFXManager.Instance.Lose();
        lostScreen.gameObject.SetActive(true);
    }

    public void RestartLevel()
    {
        GameManager.Instance.RestartLevel();
    }

    public void GoToHome()
    {
        GameManager.Instance.GoBackToHome();
    }

    void SendStatusToGameManager(LevelCompletionStatus status)
    {
        GameManager.Instance.LevelEnded(status);
        Instantiate(EndLevelCanvasPrefab).GetComponent<EndLevelCanvas>().FinishedLevel(status);
    }

    void StopLevel()
    {
        FindObjectOfType<PlayerController>().enabled = false;
        FindObjectOfType<BossController>().enabled = false;
        FindObjectOfType<StressBar>().enabled = false;
    }

    public void LoseFirstStar() { lvlStatus.hasFirstStar = false; }
    public void LoseSecondStar() { lvlStatus.hasSecondStar = false; }
    public void WinThirdStar() { lvlStatus.hasThirdStar = true; }
}
