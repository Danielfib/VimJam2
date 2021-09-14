using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] Animator animator;
    [SerializeField, Range(0, 1)] float stress;
    [SerializeField] float defaultStressVelocity = 0.001f;
    [SerializeField] SpriteRenderer spriteRenderer;

    private float currentStressVelocity;

    float stressMultiplier = 1f;
    Material playerMat;
    const string STRESS_PROP_NAME = "Vector1_f3fe65c05acb434ca3acf38395a1925e";
    int shaderStressPropNameId;

    private void Awake()
    {
        playerMat = spriteRenderer.material;
        shaderStressPropNameId = playerMat.shader.GetPropertyNameId(playerMat.shader.FindPropertyIndex(STRESS_PROP_NAME));
        currentStressVelocity = defaultStressVelocity;
    }

    void FixedUpdate()
    {
        ComputeStress();
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

    #region Stress
    private void UpdatedStress(float s)
    {
        playerMat.SetFloat(shaderStressPropNameId, stress);
        stressMultiplier = 1 + s;
        StressBar.Instance.SetStress(stress);
    }

    private void ComputeStress()
    {
        var clampedVelocity = Mathf.Clamp(currentStressVelocity, -1, 1);
        stress = Mathf.Clamp(stress + clampedVelocity, 0, 1);
        UpdatedStress(stress);
    }

    public void SetStressVelocity(float v)
    {
        currentStressVelocity = v;
    }

    public void ResetStressVelocity()
    {
        currentStressVelocity = defaultStressVelocity;
    }
    #endregion
}
