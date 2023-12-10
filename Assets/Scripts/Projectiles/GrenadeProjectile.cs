using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public int bounceCount;

    public float speed;
    public float explosionRadius;

    public GameObject particleHandler;

    private void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    private void Update()
    {
        if (bounceCount <= 0)
        {
            Explode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        bounceCount--;
    }

    void Explode()
    {
        Instantiate(particleHandler, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            nearbyObject.GetComponent<IEnemyBasic>()?.RecieveDamage(50);
        }

        Destroy(gameObject);
    }
}
