using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeItem : MonoBehaviour
{
    public delegate void IncreaseHealth();
    public static IncreaseHealth increaseHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (increaseHealth != null)
                increaseHealth();
            Destroy(this.gameObject);
        }
    }
}
