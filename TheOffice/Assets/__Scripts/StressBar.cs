using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressBar : Singleton<StressBar>
{
    [SerializeField] private RawImage[] units;
    [SerializeField] private Animator animator;
    
    private float stress;

    //s ranges from 0 to 1
    public void SetStress(float s)
    {
        stress = s;
        animator.SetFloat("Stress", stress);
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
}
