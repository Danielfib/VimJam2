using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorkstation : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BossController boss = FindObjectOfType<BossController>();
            boss.PlayerGotBackToWork();
        }
    }
}
