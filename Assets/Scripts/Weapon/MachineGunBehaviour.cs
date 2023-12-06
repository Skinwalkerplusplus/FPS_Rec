using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunBehaviour : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(TimerCoroutine());
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopAllCoroutines();
        }
    }

    IEnumerator TimerCoroutine()
    {
        while (true)
        {
            animator.SetBool("IsFiring", true);
            yield return new WaitForSeconds(0.1f);
            Debug.Log("hello");
            animator.SetBool("IsFiring", false);

        }
        
    }
}
