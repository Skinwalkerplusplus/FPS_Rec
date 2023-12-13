using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyBehavior : MonoBehaviour, IEnemyBasic
{
    public float sightRange = 10f;
    public float hearingRange = 5f;
    public float attackRange = 20f;
    public LayerMask playerLayer;
    public Transform player;
    private bool playerDetected = false;
    private bool isDead = false;

    public Transform[] patrolPoints;
    private int currentPatrolPointIndex = 0;
    private bool isPatrolling = false;

    private NavMeshAgent agent;

    public float enemyHealth;

    public delegate void EnemyAttack();
    public static EnemyAttack enemyAttack;

    GameManager gm;

    public float timeLimit = 60f;

    private Coroutine timerCoroutine;

    public bool canAttack = true;

    public GameObject enemyProjectile;

    public delegate void EnemyDeath2(int score);
    public static EnemyDeath2 enemyDeath2;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (patrolPoints.Length > 0)
        {
            isPatrolling = true;
            agent.SetDestination(patrolPoints[currentPatrolPointIndex].position);
        }
    }

    void Update()
    {
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
            if((Vector3.Distance(transform.position, player.position) <= attackRange)&&(playerDetected))
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

        if (Vector3.Distance(transform.position, player.position) <= sightRange)
        {

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

        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPointIndex].position) <= 0.1f)
        {

            currentPatrolPointIndex++;
            if (currentPatrolPointIndex >= patrolPoints.Length)
            {
                currentPatrolPointIndex = 0;
            }
            SetDestination(patrolPoints[currentPatrolPointIndex]);
        }
    }

    void SetDestination(Transform destination)
    {
        
    }

    void ChasePlayer()
    {
        if ((player != null) && (isDead == false))
        {
            agent.SetDestination(player.position);
        }
    }

    public void Attack()
    {
        Debug.Log("Attacking");
        enemyProjectile = Instantiate(enemyProjectile, transform.position, Quaternion.identity);
        
    }

    public void RecieveDamage(float damageTaken)
    {
        Debug.Log("Iamthestormthatisapproaching");
        enemyHealth -= damageTaken;
    }

    void EnemyDeath()
    {
        if (enemyDeath2 != null)
            enemyDeath2(30);
        isDead = true;
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
