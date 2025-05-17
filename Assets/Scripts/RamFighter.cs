using System.Collections;
using UnityEngine;

public class RamFighter : MonoBehaviour
{
    public string ramName;
    public Animator animator;
    public GoatStats stats;
    public RamFighter opponent;
    public Transform damagePopupPoint;

    private float timer;
    private bool isFighting = true;
    private Rigidbody rb;
    private CapsuleCollider col;
    private Vector3 camPos;
    void Start()
    {
        animator.applyRootMotion = false;

        timer = Random.Range(1f, 2f);
        camPos = Camera.main.transform.position;
        // Add Rigidbody and CapsuleCollider for physics
        rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = false; // Ensure physics works
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
            timer = Random.Range(1.5f, 2.5f);
        }
    }

    void Attack()
    {
        int baseDamage = Random.Range(10, 20);
        bool isCritical = Random.value < 0.2f;
        if (isCritical) baseDamage *= 3;

        int attackType = Random.Range(0, 2);
        string anim = attackType == 0 ? "Attack1" : "Attack2";
        animator.Play(anim);

        // Knockback opponent a short distance
        Vector3 direction = (opponent.transform.position - transform.position).normalized;
        opponent.rb.AddForce(direction * 300f); // Adjust for punchiness

        opponent.TakeDamage(baseDamage);

        // Return both fighters to original position after short delay
        StartCoroutine(ResetPositionAfterHit());
        //opponent.StartCoroutine(opponent.ResetPositionAfterHit());

        // Check win/lose
        if (opponent.stats.health <= 0)
        {
            animator.Play("Winner");
            opponent.animator.Play("Die");
            opponent.StopFighting();
            StopFighting();
        }
    }

    public void TakeDamage(int amount)
    {
        // Damage receiver loses health
        stats.TakeDamage(amount);
        UIManager.Instance.UpdateHealthBars();
        GameManager.Instance.ShowDamage(damagePopupPoint.position, amount, Color.red, false); // false = no +

        // Attacker gains health
        if (opponent != null && opponent.stats.health > 0 && opponent.stats.health < opponent.stats.maxHealth)
        {
            opponent.stats.Heal(amount);
            UIManager.Instance.UpdateHealthBars();
            GameManager.Instance.ShowDamage(opponent.damagePopupPoint.position, amount, Color.green, true); // true = show +
        }
    }


    public void StopFighting()
    {
        isFighting = false;
        rb.linearVelocity = Vector3.zero;
    }



    void OnCollisionEnter(Collision collision)
    {

    }
    IEnumerator ResetPositionAfterHit()
    {
        opponent.animator.Play("Combat_Stun");
        yield return new WaitForSeconds(0.5f); // Wait for knockback to settle
        if (isFighting && stats.health > 0)
        {
            float fixedDistance = 1.5f; // desired distance between rams

            // Compute direction and target position at fixed distance
            Vector3 dirToOpponent = (opponent.transform.position - transform.position).normalized;
            Vector3 midPoint = (transform.position + opponent.transform.position) / 3f;

            // Position this ram behind the midpoint
            Vector3 targetPos = midPoint - dirToOpponent * (fixedDistance / 3f);
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
