using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressBar : Singleton<StressBar>
{
    [SerializeField] Slider stressSlider;

    public void SetStress(float s)
    {
        stressSlider.value = s;
    }
}
