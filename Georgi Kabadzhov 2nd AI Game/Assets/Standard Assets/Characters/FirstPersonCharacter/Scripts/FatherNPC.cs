using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FatherNPC : MonoBehaviour
{

    public Transform[] waypoints;    

    NavMeshAgent agent;
    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();        
        agent.SetDestination(waypoints[counter].position);
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                counter++;
                agent.SetDestination(waypoints[counter].position);
            }
        }
    }
}
