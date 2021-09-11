using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelCanvas : MonoBehaviour
{
    public void FinishedLevel(LevelCompletionStatus status)
    {
        SetupScreenWithStatus(status);
        OpenScreen();
    }

    private void SetupScreenWithStatus(LevelCompletionStatus status)
    {
        
    }

    void OpenScreen()
    {
        gameObject.SetActive(true);
    }

    public void NextLevel()
    {
        GameManager.Instance.GoBackToHome();
    }

    public void GoToHome()
    {
        GameManager.Instance.GoBackToHome();
    }
}
