using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField]
    AudioSource audioSrc;

    [SerializeField]
    AudioClip menuMusic;

    [SerializeField]
    AudioClip[] gameMusics;

    int currentLevel = 0;

    public void Awake()
    {
        base.Awake();
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
        } else if (currentLevel == 0)
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
        int r = Random.Range(0, gameMusics.Length);
        var chosenClip = gameMusics[r];
        Play(chosenClip);
    }

    public IEnumerator MuteFor(float seconds)
    {
        var prevVol = audioSrc.volume;
        audioSrc.volume = 0;
        yield return new WaitForSeconds(seconds);
        audioSrc.volume = prevVol;
    }
}
