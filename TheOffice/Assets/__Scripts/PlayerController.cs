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
    [SerializeField] float defaultStressVelocity = 0.001f, stressVelocityWhenBossComplaining = 0.003f;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] ParticleSystem stepPS, stressSmokePS;
    [SerializeField] NavMeshAgent navAgent;

    [HideInInspector]
    public bool IsRelaxing
    {
        get { return currentStressVelocity < 0; }
    }

    private float currentStressVelocity;

    float stressMultiplier = 1f;
    Material playerMat;
    const string STRESS_PROP_NAME = "Vector1_f3fe65c05acb434ca3acf38395a1925e";
    int shaderStressPropNameId;
    int stepPSFlipFlop = -1;

    bool isControllable = true;
    bool isBeingComplained = false;

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
        if (!isBeingComplained)
        {
            currentStressVelocity = v;
        }
    }

    public void ResetStressVelocity()
    {
        if (!isBeingComplained)
        {
            StressBar.Instance.LeftArea();
            currentStressVelocity = defaultStressVelocity;
        }
    }

    internal void EnteredInStressArea(float stressInduce)
    {
        if (!isBeingComplained)
        {
            if (stressInduce > 0) StressBar.Instance.IndicateStressRaising();
            if (stressInduce < 0) StressBar.Instance.IndicateStressLowering();
            SetStressVelocity(stressInduce);
        }
    }

    internal void DetectedByBoss()
    {
        isBeingComplained = true;
        currentStressVelocity = stressVelocityWhenBossComplaining;
        StressBar.Instance.IndicateStressRaising();
    }

    public void ReleasedByBoss()
    {
        isBeingComplained = false;
        currentStressVelocity = defaultStressVelocity;
        StressBar.Instance.LeftArea();

        //relieve stress
        //TODO: play relieve sound
        stress = 0.7f * stress;
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
            boss.PlayerLost();
            navAgent.enabled = true;
            var path = new NavMeshPath();
            navAgent.CalculatePath(boss.transform.position, path);
            GetComponent<Chaser>().Chase(path.corners, speed * 2);
            navAgent.enabled = false;
            transform.localEulerAngles = Vector3.zero;
        }
    }
    #endregion
}
