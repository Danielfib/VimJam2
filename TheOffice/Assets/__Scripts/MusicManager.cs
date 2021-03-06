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

    [SerializeField]
    AudioClip bossMusic;

    float prevVol;

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
        
        if(level == 0) //main menu
        {
            Play(menuMusic);
        } else if (level == SceneManager.sceneCountInBuildSettings - 2) //last level
        {
            Play(bossMusic);
        }
        else if(level == SceneManager.sceneCountInBuildSettings) //ending screen
        {
            PlayRandomMusic();
        } else //normal levels
        {
            PlayRandomMusic();
        }
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
        prevVol = audioSrc.volume;
        audioSrc.volume = 0;
        yield return new WaitForSeconds(seconds);
        ReturnMusic();
    }

    public void ReturnMusic()
    {
        audioSrc.volume = prevVol;
    }
}
