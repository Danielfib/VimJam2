using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] Animator animator;
    [SerializeField, Range(0, 1)] float stress;
    [SerializeField] float defaultStressVelocity = 0.001f;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] ParticleSystem stepPS, stressSmokePS;
    [SerializeField] NavMeshAgent navAgent;

    private float currentStressVelocity;

    float stressMultiplier = 1f;
    Material playerMat;
    const string STRESS_PROP_NAME = "Vector1_f3fe65c05acb434ca3acf38395a1925e";
    int shaderStressPropNameId;
    int stepPSFlipFlop = -1;

    bool isControllable = true;

    private void Awake()
    {
        playerMat = spriteRenderer.material;
        shaderStressPropNameId = playerMat.shader.GetPropertyNameId(playerMat.shader.FindPropertyIndex(STRESS_PROP_NAME));
        currentStressVelocity = defaultStressVelocity;
    }

    void FixedUpdate()
    {
        if (isControllable)
        {
            ComputeStress();
            ProccessMovement();
            Animate();
        }
    }

    private void Animate()
    {
        animator.SetBool("IsWalking", rb.velocity != Vector2.zero);
        animator.speed = Mathf.Clamp(rb.velocity.magnitude / 4, 1, 2) * stressMultiplier;
    }

    public void Stepped()
    {
        int r = Random.Range(0, 100);
        if (r < 40f)
        {
            stepPS.transform.localScale = new Vector3(stepPSFlipFlop, 1, 1);
            stepPSFlipFlop *= -1;
            stepPS.Play();
        }
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
        UpdateStressSmokeParticles();
    }

    private void ComputeStress()
    {
        var clampedVelocity = Mathf.Clamp(currentStressVelocity, -1, 1);
        stress = Mathf.Clamp(stress + clampedVelocity, 0, 1);
        UpdatedStress(stress);
        CheckIfIsLosingControl();
    }

    private void CheckIfIsLosingControl()
    {
        if (stress >= 1)
            StartCoroutine(StartLosingControl());
        else
            StopCoroutine(StartLosingControl());
    }

    public void SetStressVelocity(float v)
    {
        currentStressVelocity = v;
    }

    public void ResetStressVelocity()
    {
        currentStressVelocity = defaultStressVelocity;
    }

    private void UpdateStressSmokeParticles()
    {
        var em = stressSmokePS.emission;
        em.rateOverTime = stress > 0.7f ? 10 * stress : 0;
    }

    private IEnumerator StartLosingControl()
    {
        yield return new WaitForSeconds(3f);
        LostControl();
    }

    private void LostControl()
    {
        if (isControllable)
        {
            isControllable = false;

            animator.SetBool("IsWalking", true);
            animator.speed = 2;
            BossController boss = FindObjectOfType<BossController>();
            navAgent.enabled = true;
            var path = new NavMeshPath();
            navAgent.CalculatePath(boss.transform.position, path);
            GetComponent<Chaser>().Chase(path.corners, speed);
            navAgent.enabled = false;
            transform.localEulerAngles = Vector3.zero;
        }
    }
    #endregion
}
