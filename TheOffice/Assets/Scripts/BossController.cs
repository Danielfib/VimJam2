using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossController : MonoBehaviour
{
    NavMeshAgent nma;

    private void Start()
    {
        nma = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        ChasePlayer();
    }

    public void ChasePlayer()
    {
        PlayerController player = GameObject.FindObjectOfType<PlayerController>();
        nma.SetDestination(player.transform.position);
    }
}
