using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StressInducingArea : MonoBehaviour
{
    [SerializeField] float stressInduce = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            StressBar.Instance.SetStressVelocity(stressInduce);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StressBar.Instance.ResetStressVelocity();
        }
    }
}
