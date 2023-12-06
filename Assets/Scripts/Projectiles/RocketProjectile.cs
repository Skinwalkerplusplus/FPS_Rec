using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectile : MonoBehaviour
{
    public float speed;
    public float explosionRadius;

    //public ParticleSystem explosionEffect;

    //public ParticleManager particleManager;

    public GameObject particleHandler;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = -transform.up * speed;
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
        
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
