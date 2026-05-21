using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float damage = 20f; // increases infection rate
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask targetLayer;

    private Transform target;
    private bool isDead;
    private float attackTimer;

    private void Update()
    {
        if (isDead) return;

        attackTimer -= Time.deltaTime;

        FindTarget();

        if (target == null)
        {
            Idle();
        } else
        {
            float dist = Vector3.Distance(transform.position, target.position);

            if (dist <= attackRange)
            {
                TryAttack();
            }
            else
            {
                ChaseTarget();
            }
        }

        UpdateAnimSpeed();
    }

    private void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, targetLayer); // change to direct reference
        target = hits.Length > 0 ? hits[0].transform : null;
    }

    private void ChaseTarget()
    {
        navAgent.SetDestination(target.position);
    }

    private void TryAttack()
    {
        navAgent.ResetPath(); // stop sliding into target

        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0f;
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
        }

        if (attackTimer > 0f) return;

        attackTimer = attackCooldown;
        animator.SetTrigger("Attack");
    }

    private void Idle()
    {
        navAgent.ResetPath();
        UpdateAnimSpeed();
    }

    private void UpdateAnimSpeed()
    {
        float speed = navAgent.velocity.magnitude;
        animator.SetFloat("Speed", speed);        
    }
}
