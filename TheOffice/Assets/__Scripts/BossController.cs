using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossController : MonoBehaviour
{
    [SerializeField] float stoppingDistanceFromPlayer;
    NavMeshAgent nma;

    BOSS_STATE state;

    private void Start()
    {
        nma = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        PeformStateMovement();
    }

    void PeformStateMovement()
    {
        switch (state)
        {
            case BOSS_STATE.NORMAL:
                break;
            case BOSS_STATE.CHASING_PLAYER:
                ChasePlayer();
                break;
            case BOSS_STATE.GOING_BACK_TO_NORMAL:
                break;
        }
    }

    void Patrol()
    {
        //TODO
    }

    void ChasePlayer()
    {
        PlayerController player = GameObject.FindObjectOfType<PlayerController>();
        if((player.transform.position - transform.position).magnitude > stoppingDistanceFromPlayer)
        {
            nma.isStopped = false;
            nma.SetDestination(player.transform.position);
        } else
        {
            nma.isStopped = true;
        }
    }

    void ReturnToPatrol()
    {
        //TODO
    }
}

enum BOSS_STATE
{
    NORMAL,
    CHASING_PLAYER, 
    GOING_BACK_TO_NORMAL
}