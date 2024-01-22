using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        WeaponSwitcher.weaponFired += Fire;
        WeaponSwitcher.weaponFiredStop += StopFiring;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            animator.SetBool("IsAiming", true);
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            animator.SetBool("IsAiming", false);
        }

        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    animator.SetBool("IsFiring", true);
        //}

        //if (Input.GetKeyUp(KeyCode.Mouse0))
        //{
        //    animator.SetBool("IsFiring", false);
        //}
    }

    void Fire()
    {
        if (animator != null)
        {
            animator.SetBool("IsFiring", true);
        }
        
    }

    void StopFiring()
    {
        if (animator != null)
        {
            animator.SetBool("IsFiring", false);
        }
        
    }

}
