using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockPointer : MonoBehaviour
{
    [SerializeField] float realTime, initRot, endRot;

    float timeCounter = 0;
    bool didFinish = false;

    private void FixedUpdate()
    {
        if (didFinish) return;

        if(timeCounter <= realTime)
        {
            timeCounter += Time.deltaTime;
            var desiredRot = Mathf.Lerp(initRot, endRot, timeCounter / realTime);
            transform.eulerAngles = new Vector3(0, 0, desiredRot);
        } else
        {
            didFinish = true;
            LevelManager.Instance.FinishedLevel();
        }
    }
}
