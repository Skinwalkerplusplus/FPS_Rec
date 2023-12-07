using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLProjectile : MonoBehaviour
{
    public int bounceCount;

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
        
    }
}
