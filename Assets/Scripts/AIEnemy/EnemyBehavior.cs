using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour, IEnemyBasic
{
    public float sightRange = 10f;
    public float hearingRange = 5f;
    public float attackRange = 3f;
    public LayerMask playerLayer;
    public Transform player;
    private bool playerDetected = false;

    public GameObject[] patrolPoints;
    private Transform[] patrolPointLoc;
    private int currentPatrolPointIndex = 0;
    private bool isPatrolling = false;

    private NavMeshAgent agent;

    public float enemyHealth;

    public delegate void EnemyAttack();
    public static EnemyAttack enemyAttack;

    public delegate void EnemyDeath1(int score);
    public static EnemyDeath1 enemyDeath1;

    GameManager gm;

    public float timeLimit = 60f;

    private Coroutine timerCoroutine;

    public bool canAttack = true;

    public Animator animator;

    public GameObject[] enemyPrefabs;


    void Start()
    {
        //patrolPoints = GameObject.FindObjectsOfType<PatrolPoint>();

        agent = GetComponent<NavMeshAgent>();

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPointLoc[i] = patrolPoints[i].transform;
        }

        if (patrolPoints.Length > 0)
        {
            // Start patrolling if there are patrol points defined
            isPatrolling = true;
            agent.SetDestination(patrolPoints[currentPatrolPointIndex].transform.position);
        }
    }

    void Update()
    {
        animator.SetFloat("Speed", agent.speed);

        if (enemyHealth <= 0)
        {
            EnemyDeath();
        }

        if (!playerDetected)
        {
            DetectPlayerBySight();
            DetectPlayerBySound();
            if (isPatrolling)
            {
                Patrol();
            }
        }

        else
        {
            ChasePlayer();
        }

        if (canAttack)
        {
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                StartCoroutine(TimerCoroutine());
            }
            else
            {
                StopCoroutine(TimerCoroutine());
            }
        }

        
    }

    void DetectPlayerBySight()
    {
        // Check if the player is within sight range
        if (Vector3.Distance(transform.position, player.position) <= sightRange)
        {
            // Perform line of sight check
            RaycastHit hit;
            if (Physics.Linecast(transform.position, player.position, out hit, playerLayer))
            {
                // Check if the linecast hits the player
                if (hit.collider.CompareTag("Player"))
                {
                    PlayerDetected();
                }

                else
                {
                    PlayerLost();
                }
            }


        }
    }

    void DetectPlayerBySound()
    {

        if (Vector3.Distance(transform.position, player.position) <= hearingRange)
        {

        }
    }

    void PlayerDetected()
    {

        playerDetected = true;
        Debug.Log("Player detected!");
    }

    void PlayerLost()
    {
        playerDetected = false;
        Debug.Log("Player lost!");
    }

    void Patrol()
    {

        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPointIndex].transform.position) <= 0.1f)
        {

            currentPatrolPointIndex++;
            if (currentPatrolPointIndex >= patrolPoints.Length)
            {
                currentPatrolPointIndex = 0;
            }
            SetDestination(patrolPoints[currentPatrolPointIndex].transform);
        }
    }

    void SetDestination(Transform destination)
    {
    
    }

    void ChasePlayer()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    public void Attack()
    {
        Debug.Log("Attack");
        if (enemyAttack != null)
            enemyAttack();
    }

    public void RecieveDamage(float damageTaken)
    {
        enemyHealth -= damageTaken;
    }

    void EnemyDeath()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyPrefab = enemyPrefabs[randomIndex];

        if (enemyDeath1 != null)
            enemyDeath1(20);
        Destroy(this.gameObject);
    }

    IEnumerator TimerCoroutine()
    {
        canAttack = false;
        Attack();
        yield return new WaitForSeconds(2);
        canAttack = true;
    }
}
