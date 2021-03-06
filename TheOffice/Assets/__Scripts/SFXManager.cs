using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : Singleton<SFXManager>
{
    [SerializeField] AudioSource src;
    [SerializeField] 
    AudioClip bossAlert, bossChasing, bossDeath,
    playerLostMind, playerHitsBoss, playerAlertSiren, 
    lose, stressRelief;

    [SerializeField]
    AudioClip buttonPress;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    void Play(AudioClip clip, float vol = 0.8f) { src.PlayOneShot(clip, vol); }

    public void BossAlerted() { Play(bossAlert, 0.1f); }
    public void BossChasing() { Play(bossChasing); }

    public void PlayerLostMind() { Play(playerLostMind); }
    public void PlayerHitsBoss() { Play(playerHitsBoss); Play(bossDeath); }
    public void PlayerAlertSiren() { Play(playerAlertSiren); }

    public void Lose() { Play(lose); }
    public void StressRelief() { Play(stressRelief); }

    public void ButtonPress() { Play(buttonPress); }
}
