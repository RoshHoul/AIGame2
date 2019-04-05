using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//Defender class. Randomly chooses which cannon to trigger and runs to it. Never stops
public class Defender : MonoBehaviour
{

    private GameObject[] Cannons;
    private Transform goal;
    private NavMeshAgent agent;
    private Animator anim;

    private enum State { Walking, Idle} 

    private State state;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        state = State.Walking;
        agent = GetComponent<NavMeshAgent>();
        Cannons = GameObject.FindGameObjectsWithTag("Cannon");
        goal = Cannons[Random.Range(0, Cannons.Length)].transform;
        agent.SetDestination(goal.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Walking) {
            if (goal == null) {
                goal = Cannons[Random.Range(0, Cannons.Length)].transform;
                agent.SetDestination(goal.position);
            }

            if (agent.remainingDistance <= agent.stoppingDistance) {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0) {
                    state = State.Idle;
                }
            }
        } else if (state == State.Idle) {
            //TODO change animations
            goal = null;
            state = State.Walking;

        }

    }
}
