using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockPointer : MonoBehaviour
{
    [SerializeField] float realTime, initRot, endRot;
    [Range(0, 1), SerializeField] float beginTickTockAt;
    [SerializeField] AudioSource audioSrc;

    float timeCounter = 0;
    bool didFinish = false;

    private void FixedUpdate()
    {
        if (didFinish) return;

        CheckIfNearEnd();
        if (timeCounter <= realTime)
        {
            timeCounter += Time.deltaTime;
            var desiredRot = Mathf.Lerp(initRot, endRot, timeCounter / realTime);
            transform.eulerAngles = new Vector3(0, 0, desiredRot);
        } else
        {
            audioSrc.enabled = false;
            didFinish = true;
            LevelManager.Instance.FinishedLevel();
        }
    }

    void CheckIfNearEnd()
    {
        if (timeCounter / realTime > beginTickTockAt)
        {
            audioSrc.enabled = true;
        }
    }
}
