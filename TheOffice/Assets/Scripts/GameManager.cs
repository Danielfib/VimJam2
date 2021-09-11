using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] int[] lvlsBuildIds;
    int currentLevel = 1;

    LevelCompletionStatus[] stats;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        stats = new LevelCompletionStatus[lvlsBuildIds.Length];
    }

    public void Nextlevel() 
    {
        currentLevel++;
        SceneManager.LoadScene(currentLevel);
    }

    public void GoBackToHome()
    {
        SceneManager.LoadScene(0);
    }

    public void LevelEnded(LevelCompletionStatus status)
    {

    }
}
