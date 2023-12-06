using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBehabior : MonoBehaviour
{
    public Camera fpsCamera;
    public int pelletsPerShot = 8;
    public float spreadAngle = 10f;
    public float fireRate = 1f;
    public float damage = 10f;
    public float maxRange = 100000f;
    public LayerMask targetLayer;
    //public ParticleSystem muzzleFlash;

    private bool canShoot = true;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            Debug.Log("disparo");
                
            Shoot();
        }
    }

    void Shoot()
    {
        StartCoroutine(ShotCooldown());

        for (int i = 0; i < pelletsPerShot; i++)
        {
            // Apply spread to the ray direction
            Vector3 spread = Random.insideUnitCircle * spreadAngle;
            Vector3 direction1 = fpsCamera.transform.forward + spread.x * fpsCamera.transform.right + spread.y * fpsCamera.transform.up;

            // Shoot a raycast to detect hits
            Ray ray = new Ray(transform.position, direction1);
            RaycastHit hit;
            if (Physics.Raycast(fpsCamera.transform.position, direction1, out hit, maxRange, targetLayer))
            {
                Debug.Log("Hit target: " + hit.collider.gameObject.name);
                if (hit.collider.CompareTag("Enemy"))
                {
                    EnemyBehavior enemy = hit.collider.GetComponent<EnemyBehavior>();
                    if (enemy != null)
                    {
                        //enemy.TakeDamage(10f);
                    }
                }
            }
            Debug.DrawRay(transform.position, direction1);
        }

        //if (muzzleFlash != null)
        //{
        //    muzzleFlash.Play();
        //}
    }

    System.Collections.IEnumerator ShotCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(1f / fireRate);
        canShoot = true;
    }
}
