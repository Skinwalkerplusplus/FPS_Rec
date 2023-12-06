using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBase : MonoBehaviour
{
    protected NavMeshAgent cmpAgent;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        cmpAgent = GetComponent<NavMeshAgent>();
    }
}
