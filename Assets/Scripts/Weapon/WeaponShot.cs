using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShot : MonoBehaviour
{
    public Camera playerCamera;
    public float raycastDistance = 100f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireGun();
        }
    }

    void FireGun()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
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
        else
        {
            Debug.Log("Missed the target!");
        }
    }


}
