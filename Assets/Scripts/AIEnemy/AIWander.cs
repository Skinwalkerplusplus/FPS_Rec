using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWander : AIBase
{
    [SerializeField] float maxTimeToChange = 5;
    float currentTimeToChange;
    Vector3 initialPosition;
    [SerializeField] float maxRadio = 10;

    protected override void Start()
    {
        base.Start();

        initialPosition = transform.position;

        Vector3 randomPoint = initialPosition + Random.insideUnitSphere * maxRadio;
        randomPoint.y = transform.position.y;
        cmpAgent.SetDestination(randomPoint);        
    }

    // Update is called once per frame
    void Update()
    {
        currentTimeToChange += Time.deltaTime;
        if(cmpAgent.remainingDistance < 0.1f || currentTimeToChange > maxTimeToChange)
        {
            Vector3 randomPoint = initialPosition + Random.insideUnitSphere * maxRadio;
            randomPoint.y = transform.position.y;
            cmpAgent.SetDestination(randomPoint);

            currentTimeToChange = 0;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(cmpAgent.destination, 1);
    //}
}
