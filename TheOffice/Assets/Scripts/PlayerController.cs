using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] Animator animator;

    float stressMultiplier = 1f;

    void Update()
    {
        ProccessMovement();
        Animate();
    }

    private void Animate()
    {
        animator.SetBool("IsWalking", rb.velocity != Vector2.zero);
        animator.speed = Mathf.Clamp(rb.velocity.magnitude / 4, 1, 2) * stressMultiplier;
    }

    private void ProccessMovement()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(h, v) * speed;
    }
}
