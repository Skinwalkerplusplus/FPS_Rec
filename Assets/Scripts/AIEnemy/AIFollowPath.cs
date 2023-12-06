using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFollowPath : AIBase
{
    [SerializeField] GameObject[] pathPoints;
    int currentPathPoint;

    protected override void Start()
    {
        base.Start();

        cmpAgent.SetDestination(pathPoints[currentPathPoint].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(cmpAgent.remainingDistance < 0.1f)
        {
            currentPathPoint += 1;
            if(currentPathPoint == pathPoints.Length)
            {
                currentPathPoint = 0;
            }
            cmpAgent.SetDestination(pathPoints[currentPathPoint].transform.position);
        }
    }
}
