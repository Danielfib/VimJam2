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
        int stars = 0;
        if (status.hasFirstStar) stars++;
        if (status.hasSecondStar) stars++;
        if (status.hasThirdStar) stars++;

        firstStar.color = stars > 0 ? Color.white : noStarColor;
        secondStar.color = stars > 1 ? Color.white : noStarColor;
        thirdStar.color = stars > 2 ? Color.white : noStarColor;
    }

    void OpenScreen()
    {
        gameObject.SetActive(true);
        StartCoroutine(MusicManager.Instance.MuteFor(GetComponent<AudioSource>().clip.length));
    }

    public void NextLevel()
    {
        ResumeMusic();
        GameManager.Instance.NextLevel();
    }

    public void GoToHome()
    {
        ResumeMusic();
        GameManager.Instance.GoBackToHome();
    }

    public void RestartLevel()
    {
        ResumeMusic();
        GameManager.Instance.RestartLevel();
    }

    private void ResumeMusic()
    {
        MusicManager.Instance.ReturnMusic();
    }
}
