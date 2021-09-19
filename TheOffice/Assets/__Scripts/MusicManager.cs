using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSrc;

    [SerializeField]
    AudioClip menuMusic;

    [SerializeField]
    AudioClip[] gameMusics;

    int currentLevel = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded -= LoadedScene;
        SceneManager.sceneLoaded += LoadedScene;
    }

    private void LoadedScene(Scene scene, LoadSceneMode mode)
    {
        int level = scene.buildIndex;
        print(level);
        if(level == 0)
        {
            Play(menuMusic);
        } else
        {
            PlayRandomMusic();
        }

        currentLevel = level;
    }

    void Play(AudioClip clip)
    {
        audioSrc.clip = clip;
        audioSrc.Play();
    }

    void PlayRandomMusic()
    {
        int r = Random.Range(0, gameMusics.Length - 1);
        var chosenClip = gameMusics[r];
        Play(chosenClip);
    }
}
