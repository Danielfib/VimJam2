using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevelCanvas : MonoBehaviour
{
    [SerializeField]
    private Image firstStar, secondStar, thirdStar;
    private Color noStarColor = new Color(0.1f, 0.1f, 0.1f, 1f);

    public void FinishedLevel(LevelCompletionStatus status)
    {
        SetupScreenWithStatus(status);
        OpenScreen();
    }

    private void SetupScreenWithStatus(LevelCompletionStatus status)
    {
        firstStar.color = status.hasFirstStar ? Color.white : noStarColor;
        secondStar.color = status.hasSecondStar ? Color.white : noStarColor;
        thirdStar.color = status.hasThirdStar ? Color.white : noStarColor;
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

    public void RestartLevel()
    {
        GameManager.Instance.RestartLevel();
    }
}
