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
        //TODO
        homeScreen.SetActive(false);
        returnButton.SetActive(true);
        levelsScreen.SetActive(true);
    }

    public void GoToCreditsScreen()
    {
        //TODO
        homeScreen.SetActive(false);
        returnButton.SetActive(true);
        creditsScreen.SetActive(true);
    }

    public void GoToTutorialScreen()
    {
        //TODO
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
}
