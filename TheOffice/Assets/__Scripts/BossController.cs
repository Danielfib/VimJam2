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
                line.enabled = false;
                break;
        }
    }

    void UpdateLineRenderer()
    {
        //var nextI = currentPatrolPos == patrolPositions.Length - 1 ? 0 : currentPatrolPos + 1;

        //var positions = new List<Vector3>();

        //foreach (var item in nma.path.corners)
        //{
        //    positions.Add(item);
        //}
        //var midDist = patrolPositions[nextI].position - patrolPositions[currentPatrolPos].position;
        //positions.Add(patrolPositions[currentPatrolPos].position + midDist.normalized * 10);

        //line.positionCount = positions.Count;
        //line.SetPositions(positions.ToArray());

        List<Vector3> reversedPos = nma.path.corners.ToList();
        reversedPos.Reverse();

        line.positionCount = nma.path.corners.Length;
        line.SetPositions(reversedPos.ToArray());

        //var nextI = currentPatrolPos == patrolPositions.Length - 1 ? 0 : currentPatrolPos + 1;
        //var pos = new Vector3[] { transform.position, patrolPositions[currentPatrolPos].position, patrolPositions[nextI].position };
        //line.positionCount = pos.Length;
        //line.SetPositions(pos);
    }

    public void DetectedExclamation()
    {
        var exc = Instantiate(exclamationPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity, transform);
        StartCoroutine(DestroyAfter(exc, 1.5f));
        state = BOSS_STATE.CHASING_PLAYER;
        nma.speed = chasePlayerSpeed;
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
        LevelManager.Instance.CaughtByBoss();
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
}

public enum BOSS_STATE
{
    NORMAL,
    CHASING_PLAYER, 
    STOPPED
}