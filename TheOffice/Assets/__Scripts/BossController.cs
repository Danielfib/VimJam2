using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossController : MonoBehaviour
{
    [SerializeField] float stoppingDistanceFromPlayer, patrolSpeed, chasePlayerSpeed;
    [SerializeField] Fov fov;
    [SerializeField] LayerMask bossDetectable;
    [SerializeField] Animator animator;
    [SerializeField] GameObject exclamationPrefab;
    [SerializeField] GameObject pathLinePrefab;
    [SerializeField] AudioSource src;
    [SerializeField] AudioClip[] complainAudios;

    [SerializeField] Transform[] patrolPositions;
    int currentPatrolPos = 0;

    NavMeshAgent nma;
    PlayerController player;
    public BOSS_STATE state;
    LineRenderer line;

    private void Start()
    {
        nma = GetComponent<NavMeshAgent>();
        nma.speed = patrolSpeed;
        player = GameObject.FindObjectOfType<PlayerController>();
        if(line == null)
        {
            line = Instantiate(pathLinePrefab, transform).GetComponent<LineRenderer>();
        }
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
                line.enabled = true;
                Patrol();
                break;
            case BOSS_STATE.CHASING_PLAYER:
                line.enabled = false;
                FacePlayer();
                ChasePlayer();
                break;
            case BOSS_STATE.STOPPED:
                FacePlayer();
                line.enabled = false;
                break;
        }
    }

    void UpdateLineRenderer()
    {
        List<Vector3> reversedPos = nma.path.corners.ToList();
        reversedPos.Reverse();

        line.positionCount = nma.path.corners.Length;
        line.SetPositions(reversedPos.ToArray());
    }

    public void DetectedExclamation()
    {
        var exc = Instantiate(exclamationPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity, transform);
        StartCoroutine(DestroyAfter(exc, 1.5f));
        state = BOSS_STATE.CHASING_PLAYER;
        nma.speed = chasePlayerSpeed;
        StartCoroutine(ComplainAudio());
    }

    private IEnumerator DestroyAfter(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(go);
    }

    void Patrol()
    {
        nma.isStopped = false;
        UpdateLineRenderer();
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
                if (hit.collider.CompareTag("Player") && (player.IsRelaxing || player.isBusy)) //found player relaxing or playing videogame
                {
                    DetectedPlayer();
                } 
                else if (hit.collider.CompareTag("PlayerWorkspace") 
                    && (player.transform.position - hit.transform.position).magnitude > 2.5f) //found empty workspace
                {
                    DetectedPlayer();
                }
            }
        }
    }

    void DetectedPlayer()
    {
        state = BOSS_STATE.STOPPED;
        animator.SetTrigger("Detected");
        player.DetectedByBoss();
        LevelManager.Instance.CaughtByBoss();
        SFXManager.Instance.BossAlerted();
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
        nma.speed = patrolSpeed;
        animator.SetTrigger("PlayerGotBackToWork");
        player.ReleasedByBoss();
    }

    public void PlayerLost()
    {
        state = BOSS_STATE.STOPPED;
        nma.isStopped = true;
    }

    public void Die()
    {
        animator.SetTrigger("Dead");
    }

    IEnumerator ComplainAudio()
    {
        while (state == BOSS_STATE.CHASING_PLAYER)
        {
            yield return new WaitForSeconds(0.6f);
            int r = Random.Range(0, complainAudios.Length);
            src.PlayOneShot(complainAudios[r]);
        }
    }

    private void OnDisable()
    {
        nma.isStopped = true;
        StopAllCoroutines();
    }
}

public enum BOSS_STATE
{
    NORMAL,
    CHASING_PLAYER, 
    STOPPED
}