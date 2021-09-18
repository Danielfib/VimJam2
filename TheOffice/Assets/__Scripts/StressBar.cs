using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressBar : Singleton<StressBar>
{
    [SerializeField] private RawImage[] units;
    [SerializeField] private Animator barAnimator, velAnimator;
    
    private float stress;
    
    // 0 is outside area
    // -1 is inside relaxing area
    // 1 is inside stressing area
    private int stressVelocitySign;

    //s ranges from 0 to 1
    public void SetStress(float s)
    {
        stress = s;
        Animate();
        for (int i = 0; i < units.Length; i++)
        {
            var tOffset = (float)i / (float)units.Length;
            if(tOffset < s)
            {
                var unitColor = Color.Lerp(Color.green, Color.red, tOffset);
                units[i].color = unitColor;
            } else
            {
                units[i].color = Color.gray;
            }
        }
    }

    private void Animate()
    {
        barAnimator.SetFloat("Stress", stress);
        velAnimator.SetInteger("Velocity", stressVelocitySign);
    }

    public void IndicateStressLowering()
    {
        stressVelocitySign = -1;
    }

    public void IndicateStressRaising()
    {
        stressVelocitySign = 1;
    }

    public void LeftArea()
    {
        stressVelocitySign = 0;
    }
}
