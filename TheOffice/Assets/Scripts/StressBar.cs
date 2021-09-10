using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressBar : Singleton<StressBar>
{
    [SerializeField] float defaultStressVelocity = 0.001f;
    [SerializeField] Slider stressSlider;

    private float currentStressVelocity;

    private void Start()
    {
        currentStressVelocity = defaultStressVelocity;
    }

    private void FixedUpdate()
    {
        ComputeStress();
    }

    private void ComputeStress()
    {
        stressSlider.value += Mathf.Clamp(currentStressVelocity, -1, 1);
    }

    public void SetStressVelocity(float v)
    {
        currentStressVelocity = v;
    }

    public void ResetStressVelocity()
    {
        currentStressVelocity = defaultStressVelocity;
    }
}
