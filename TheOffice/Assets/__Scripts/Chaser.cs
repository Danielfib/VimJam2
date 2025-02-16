using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Chaser : MonoBehaviour
{
    [SerializeField] private GameObject bloodSplashPrefab;
    [SerializeField] private Rigidbody2D rb;
    private Vector3[] positions;
    private float speed;

    int currentPosition = 0;

    public void Chase(Vector3[] path, float s)
    {
        positions = path;
        speed = s;
        enabled = true;
    }

    void ChasePos()
    {
        rb.linearVelocity = (positions[currentPosition] - transform.position).normalized * speed;
    }

    void GotToNextPos()
    {
        if(currentPosition == positions.Length - 1)
        {
            ArrivedAtFinalPosition();
        } else
        {
            currentPosition++;
        }
    }

    private void OnDrawGizmos()
    {
        if(positions != null)
        {
            Gizmos.color = Color.red;
            foreach (var pos in positions)
            {
                Gizmos.DrawSphere(pos, 0.5f);
            }
        }
    }


    private void FixedUpdate()
    {
        if (positions == null) return;

        ChasePos();
        if (ArrivedAtPos(positions[currentPosition]))
        {
            GotToNextPos();
        }
    }

    private bool ArrivedAtPos(Vector3 pos)
    {
        return (transform.position - pos).magnitude < 1f;
    }

    private void ArrivedAtFinalPosition()
    {
        rb.linearVelocity = Vector2.zero;
        BloodSplatter();
        LoseScreen();
        enabled = false;
        SFXManager.Instance.PlayerHitsBoss();
        FindObjectOfType<BossController>().Die();
        return;
    }

    void BloodSplatter()
    {
        var splash = Instantiate(bloodSplashPrefab, null);
        var lastPos = positions[positions.Length - 1];
        splash.transform.position = lastPos;
    }

    void LoseScreen()
    {
        LevelManager.Instance.Lost();
    }
}
