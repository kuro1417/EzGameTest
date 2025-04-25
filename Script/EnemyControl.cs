using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.UI;

public class EnemyControl : MonoBehaviour
{
    //public static EnemyControl Instance { get; private set; }

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;

    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject healthBarCanvas;

    //Patroling
    private float currentHealth;
    private Vector3 walkPoint;
    private bool walkPointSet;
    private float walkPointRange;

    //Attacking
    private  float timeBetweenAttacks;
    private bool alreadyAttacked;

    //State
    private float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;

    private bool isWalking;
    private bool isAttacking;
    private bool isDead = false;

    private IObjectPool<EnemyControl> enemyPool;

    public static event Action<EnemyControl> OnEnemyDeath;
    public void SetPool(IObjectPool<EnemyControl> pool)
    {
        enemyPool = pool;
    }
    private void Awake() 
    {
        //Instance = this;

        player = GameObject.Find("CharMain_01").transform;
        agent = GetComponent<NavMeshAgent>();
        currentHealth = enemySO.maxHealth;
    }

    private void Update()
    {
        if (isDead || !agent.enabled || !agent.isOnNavMesh) return;

        playerInSightRange = Physics.CheckSphere(transform.position, enemySO.sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, enemySO.attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

        if(healthBarCanvas != null)
        {
            healthBarCanvas.transform.rotation = Quaternion.LookRotation(healthBarCanvas.transform.position - Camera.main.transform.position);
        }
    }

    private void Patrolling()
    {
        if (!agent.enabled || !agent.isOnNavMesh) return;

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;   

        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-enemySO.walkPointRange, enemySO.walkPointRange);
        float randomX = UnityEngine.Random.Range(-enemySO.walkPointRange, enemySO.walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    private void ChasePlayer()
    {
        if (!agent.enabled || !agent.isOnNavMesh) return;

        isWalking = true;
        isAttacking = false;
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        if (!agent.enabled || !agent.isOnNavMesh) return;

        isWalking = false;
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            isAttacking = true;

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), enemySO.timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0f)
        {
            Eliminate();
        }

        if (healthBar == null) return;
        healthBar.fillAmount = currentHealth/enemySO.maxHealth;
    }

    public void ResetEnemy()
    {
        isDead = false;
        currentHealth = enemySO.maxHealth;

        agent.enabled = true;
        agent.isStopped = false;
        agent.ResetPath();

        isWalking = false;
        isAttacking = false;
        walkPointSet = false;
        alreadyAttacked = false;

        if (healthBar != null)
        {
            healthBar.fillAmount = 1f;
        }

        StartCoroutine(EnsureOnNavMesh());
    }

    private IEnumerator EnsureOnNavMesh()
    {
        yield return null; 

        if (!agent.isOnNavMesh)
        {
            agent.Warp(transform.position);
        }
    }

    private void Eliminate()
    {
        if (isDead) return; 

        isDead = true;

        agent.isStopped = true;
        OnEnemyDeath?.Invoke(this);
        agent.enabled = false;

        if (TryGetComponent<Animator>(out var animator))
        {
            animator.SetTrigger("IsDead");
        }

        StartCoroutine(DelayedReturnToPool());
    }

    private IEnumerator DelayedReturnToPool()
    {
        yield return new WaitForSeconds(2f); 

        if (enemyPool != null)
        {
            enemyPool.Release(this); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsBoss()
    {
       return enemySO.isBoss;
    }
    public bool IsWalking()
    {
        return isWalking;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
}