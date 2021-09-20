using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    int currentLevel = 1;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void NextLevel() 
    {
        currentLevel++;
        SceneManager.LoadScene(currentLevel);
    }

    public void GoBackToHome()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }

    public void LoadLevel(int index)
    {
        currentLevel = index;
        SceneManager.LoadScene(currentLevel);
    }

    public void LevelEnded(LevelCompletionStatus status)
    {
        StoreStatus(status);
    }

    void StoreStatus(LevelCompletionStatus status)
    {
        int stars = 0;
        if (status.hasFirstStar) stars++;
        if (status.hasSecondStar) stars++;
        if (status.hasThirdStar) stars++;

        int prevStars = PlayerPrefs.GetInt(currentLevel.ToString());
        if(stars > prevStars)
        {
            PlayerPrefs.SetInt(currentLevel.ToString(), stars);
        }
    }
}
