using System.Collections;
using UnityEngine;
using System;
public class RamFighter : MonoBehaviour
{
    public Action<int, RamFighter> ThrowResponse = delegate { };
    public string ramName;
    [SerializeField] private Animator animator;
    public Transform damagePopupPoint;
    public GoatStats stats;
    public RamFighter opponent;

    private float timer;
    private bool isFighting = true;
    private Rigidbody rb;
    private CapsuleCollider col;

    // For Two object only i take direct ref.. of opponent otherwise must be collision obj prefer

    void Start()
    {
        animator.applyRootMotion = false;
        timer = UnityEngine.Random.Range(1f, 2f);
        rb = gameObject.GetComponent<Rigidbody>();

        rb.isKinematic = false; // physics works
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotation;


        col = gameObject.GetComponent<CapsuleCollider>();
        if (col == null) col = gameObject.AddComponent<CapsuleCollider>();
        col.center = new Vector3(0, 1, 0);
        col.height = 2f;
        col.radius = 0.5f;
    }

    void Update()
    {
        if (!isFighting || stats.health <= 0) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Attack();
            timer = UnityEngine.Random.Range(1.5f, 2.5f);
        }
    }

    void Attack()
    {
        int baseDamage = UnityEngine.Random.Range(10, 20);
        bool isCritical = UnityEngine.Random.value < 0.2f;
        if (isCritical) baseDamage *= 3;

        int attackType = UnityEngine.Random.Range(0, 2);
        string anim = attackType == 0 ? "Attack1" : "Attack2";
        animator.Play(anim);

        // Knockback opponent a short distance
        Vector3 direction = (opponent.transform.position - transform.position).normalized;
        opponent.rb.AddForce(direction * 300f); // Adjust for punchiness

        ThrowResponse?.Invoke(baseDamage, this);

        // Reset position after short delay
        StartCoroutine(ResetPositionAfterHit());

        // Check win/lose
        if (opponent.stats.health <= 0)
        {
            animator.Play("Winner");
            opponent.animator.Play("Die");
            opponent.StopFighting();
            StopFighting();
        }
    }

    public void StopFighting()
    {
        isFighting = false;
        rb.linearVelocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Plane")
        {
            // Initially Shake Camera
        }
    }

    IEnumerator ResetPositionAfterHit()
    {
        opponent.animator.Play("Combat_Stun");
        yield return new WaitForSeconds(0.5f); // Wait for knockback to settle
        if (isFighting && stats.health > 0)
        {
            float fixedDistance = 0.5f; // desired distance between rams

            // Compute direction and target position at fixed distance
            Vector3 dirToOpponent = (opponent.transform.position - transform.position).normalized;
            Vector3 midPoint = (transform.position + opponent.transform.position) / 2f;

            // Position this ram behind the midpoint
            Vector3 targetPos = midPoint - dirToOpponent * (fixedDistance / 2f);
            targetPos.y = transform.position.y;

            // Smoothly move to target position
            float t = 0;
            Vector3 startPos = transform.position;
            while (t < 1f)
            {
                t += Time.deltaTime * 3f;
                transform.position = Vector3.Lerp(startPos, targetPos, t);
                yield return null;
            }
        }
    }
}