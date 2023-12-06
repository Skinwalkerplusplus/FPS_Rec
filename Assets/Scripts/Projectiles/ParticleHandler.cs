using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    public ParticleSystem particles;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        particles.Play();
        audioSource.Play();
        StartCoroutine(End());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator End()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            Destroy(this.gameObject);
        }

    }
}
