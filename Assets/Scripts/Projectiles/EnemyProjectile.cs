using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f;

    private Rigidbody rb;
    private Transform player;

    public delegate void RangedEnemyAttack();
    public static RangedEnemyAttack rangedEnemyAttack;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found!");
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found!");
        }
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }

        StartCoroutine(TimerCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (rangedEnemyAttack != null)
                rangedEnemyAttack();
        }
    }

    IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(10);
        Destroy(this.gameObject);
    }

}
