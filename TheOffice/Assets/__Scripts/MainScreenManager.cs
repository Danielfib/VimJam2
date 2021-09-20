using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField]
    GameObject homeScreen, tutorialScreen, creditsScreen, levelsScreen, returnButton;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToLevelScreen()
    {
        homeScreen.SetActive(false);
        returnButton.SetActive(true);
        levelsScreen.SetActive(true);
        FindObjectOfType<LevelsScreenManager>().LoadLevelsStatus();
    }

    public void GoToCreditsScreen()
    {
        homeScreen.SetActive(false);
        returnButton.SetActive(true);
        creditsScreen.SetActive(true);
    }

    public void GoToTutorialScreen()
    {
        homeScreen.SetActive(false);
        returnButton.SetActive(true);
        tutorialScreen.SetActive(true);
    }

    public void BackToHome()
    {
        returnButton.SetActive(false);
        tutorialScreen.SetActive(false);
        creditsScreen.SetActive(false);
        levelsScreen.SetActive(false);
        homeScreen.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
