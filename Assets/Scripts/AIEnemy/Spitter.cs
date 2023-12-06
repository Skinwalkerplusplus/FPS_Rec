using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spitter : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent nav;
    public Transform playerTransform;
    public LayerMask whatIsGround, whatIsPlayer;
    public Vector3 walkPoint;
    public GameObject projectile;
    public Transform spawnPoint;
    private Animator animator;
    //private AudioManager fx;
    //private GameManager gm;

    [Header("Attributes")]
    public int health;
    public int points;
    public float walkPointRange;
    public float timeBetweenAttacks = 2f;
    public float timeBetweenScreams = 6f;
    public float sightRange, attackRange;
    public float shootingForceX, shootingForceY;
    public bool stationary;
    public bool isPlayerInChaseRange, isPlayerInAttackRange;
    private int counter;

    bool walkPointSet;
    bool hasAttacked = true;
    bool hasScreamed = true;

    private void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //fx = FindObjectOfType<AudioManager>();
        //gm = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        //StartCoroutine(SpitterHealthChecker());
    }

    private void Update()
    {
        if (stationary) { nav.speed = 0; }

        isPlayerInChaseRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //animator.SetFloat("Speed", nav.speed);

        if (!isPlayerInChaseRange && !isPlayerInAttackRange) { animator.SetBool("alerted", false); }
        if(isPlayerInChaseRange && !isPlayerInAttackRange) 
        { 
            animator.SetBool("alerted", true);
            if (hasScreamed)
            {
                animator.SetTrigger("scream");
                hasScreamed = false;
            }
            
        }
        if(isPlayerInChaseRange && isPlayerInAttackRange) { if (hasAttacked) { animator.SetTrigger("attack");} }
    }

    private void Patrol()
    {
        if (!walkPointSet) { LookForPoints(); }

        if(walkPointSet) 
        {            
            nav.SetDestination(walkPoint);
        }
        Vector3 distanceToWalk = transform.position - walkPoint;

        //If the point is reached
        if (distanceToWalk.magnitude < 1f) { walkPointSet = false; }
        
    }

    private void LookForPoints()
    {
        float rndmZ = Random.Range(-walkPointRange, walkPointRange);
        float rndmX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + rndmX, transform.position.y, transform.position.z + rndmZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) { walkPointSet = true; }
    }

    private void Chase()
    {
        nav.SetDestination(playerTransform.position);
    }

    private void Attack()
    {
        transform.LookAt(playerTransform);
        //SpittingSoundFx();
        Rigidbody rb = Instantiate(projectile, spawnPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * shootingForceX, ForceMode.Impulse);
        rb.AddForce(transform.up * shootingForceY, ForceMode.Impulse);
        hasAttacked = false;
        StartCoroutine(FireRate());
    }

    //public int GetScore()
    //{
    //    if (gm.doubleXP)
    //    {
    //        //StartCoroutine(PowerUpTimer());
    //        return gm.ExperienceMultiplier(gm.spitter_XP, 2);
    //    }
    //    else { return gm.spitter_XP; }
    //}

    //private void SpittingSoundFx()
    //{
    //    int play = Random.Range(0, 2);

    //    switch (play)
    //    {
    //        case 0:
    //            fx.PlayClipAtPoint("spit1", this.transform.position);
    //            break;
    //        case 1:
    //            fx.PlayClipAtPoint("spit2", this.transform.position);
    //            break;
    //        case 2:
    //            fx.PlayClipAtPoint("spit3", this.transform.position);
    //            break;
    //    }
    //}

    //private void SpitterScream() { fx.PlayClipAtPoint("spitterScream", this.transform.position); }

    //private void ScreamCorr()
    //{
    //    StartCoroutine(ScreamRate());
    //}

    public void RecieveDamage(int damage)
    {
        health -= damage;
        Debug.Log("health");        
    }

    //private void HealthChecker() 
    //{ 
    //    if (health <= 0) 
    //    {
    //        gm.money += points;
    //        animator.SetTrigger("die"); 
    //    } 
    //}

    public void SelfDestroy() { Destroy(this.gameObject); }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    IEnumerator FireRate()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        hasAttacked = true;
    }

    IEnumerator ScreamRate()
    {
        if (!hasScreamed)
        {
            yield return new WaitForSeconds(timeBetweenScreams);
            hasScreamed = true;
        }
    }


    public void ResetCounter() { counter = 0; }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    //IEnumerator SpitterHealthChecker()
    //{
    //    yield return new WaitUntil(() => health <= 0);
    //    animator.SetTrigger("die");
    //    gm.playerScore += GetScore();
    //    yield return null;
    //}
}
