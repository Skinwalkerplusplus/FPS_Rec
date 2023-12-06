using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSPlayer : MonoBehaviour
{
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
        if (other.tag == "item")
        {
            other.GetComponent<IItemExecutable>()?.EnteredByPlayer();
        }
        
        if(other.tag== "areaEffect")
        {
            other.GetComponent<IAreaEffectPlayer>()?.PlayerEnter();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "areaEffect")
        {
            other.GetComponent<IAreaEffectPlayer>()?.PlayerExit();
        }
    }
}
