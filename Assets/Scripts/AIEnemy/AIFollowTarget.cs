using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIFollowTarget : AIBase
{
    [SerializeField] GameObject target;
    
    // Update is called once per frame
    void Update()
    {
        cmpAgent.SetDestination(target.transform.position);
    }
}
