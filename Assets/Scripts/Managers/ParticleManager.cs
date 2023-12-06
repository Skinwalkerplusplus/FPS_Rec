using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem explosionEffect;

    public GameObject particleContainer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explosion(Transform explosionLocation)
    {
        particleContainer.transform.position = explosionLocation.position;

        if (explosionEffect != null)
        {
            explosionEffect.Play();
        }
    }
}
