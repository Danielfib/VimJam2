using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossController : MonoBehaviour
{
    [SerializeField] float stoppingDistanceFromPlayer;
    [SerializeField] Fov fov;
    [SerializeField] LayerMask bossDetectable;
    [SerializeField] Animator animator;

    [SerializeField] Transform[] patrolPositions;
    int currentPatrolPos = 0;

    NavMeshAgent nma;
    PlayerController player;
    BOSS_STATE state;

    private void Start()
    {
        nma = GetComponent<NavMeshAgent>();
        player = GameObject.FindObjectOfType<PlayerController>();
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
                Patrol();
                break;
            case BOSS_STATE.CHASING_PLAYER:
                FacePlayer();
                ChasePlayer();
                break;
            case BOSS_STATE.GOING_BACK_TO_NORMAL:
                break;
        }
    }

    public void DetectedExclamation()
    {
        //TODO: spawn exclamation point and make noise
        state = BOSS_STATE.CHASING_PLAYER;
    }

    void Patrol()
    {
        nma.isStopped = false;
        CheckForWrongThings();
        nma.SetDestination((Vector2)patrolPositions[currentPatrolPos].position);
        if((transform.position - patrolPositions[currentPatrolPos].position).magnitude < 1)
        {
            if (currentPatrolPos == patrolPositions.Length - 1)
                currentPatrolPos = 0;
            else
                currentPatrolPos++;
        }   
    }

    void CheckForWrongThings()
    {
        for (var i = 0; i <= fov.fovDensity; i++)
        {
            var angle = ((float)i / (float)fov.fovDensity) * (2 * Mathf.PI) * Mathf.Rad2Deg;
            var dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, fov.fovDistance, bossDetectable.value);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player") && player.IsRelaxing) //found player relaxing
                {
                    DetectedPlayer();
                } 
                else if (hit.collider.CompareTag("PlayerWorkspace") 
                    && (player.transform.position - hit.transform.position).magnitude > 2) //found empty workspace
                {
                    DetectedPlayer();
                }
            }
        }
    }

    void DetectedPlayer()
    {
        animator.SetTrigger("Detected");
        player.DetectedByBoss();
    }

    public void PlayerGotBackToWork()
    {
        ReturnToPatrol();
    }

    void ChasePlayer()
    {
        if ((player.transform.position - transform.position).magnitude > stoppingDistanceFromPlayer)
        {
            nma.isStopped = false;
            nma.SetDestination(player.transform.position);
        } else
        {
            nma.isStopped = true;
        }
    }

    void FacePlayer()
    {
        if((player.transform.position - transform.position).x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        } else
        {
            transform.localScale = Vector3.one;
        }
    }

    void ReturnToPatrol()
    {
        state = BOSS_STATE.NORMAL;
        animator.SetTrigger("PlayerGotBackToWork");
        player.ReleasedByBoss();
    }
}

enum BOSS_STATE
{
    NORMAL,
    CHASING_PLAYER, 
    GOING_BACK_TO_NORMAL
}