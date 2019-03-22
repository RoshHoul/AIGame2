using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Peasant : MonoBehaviour
{

    public Transform goal;
    public Transform previousGoal;
    NavMeshAgent agent;
    AICrowdManager crowdManager;

    public enum State {
        IDLE,
        WALKING,
    }

    public State state;    

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        crowdManager = GameObject.Find("CrowdManager").GetComponent<AICrowdManager>();
        state = State.WALKING;
        
        if (goal == null) {
            goal = crowdManager.GoalRequest(null);
        }
        //goal = crowdManager.GoalRequest(previousGoal);
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.WALKING) {
            if (goal == null) {
                goal = crowdManager.GoalRequest(previousGoal);    
            }
            agent.SetDestination(goal.transform.position);        
            
            if (agent.remainingDistance <= agent.stoppingDistance) {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                    previousGoal = goal;
                    if (previousGoal.gameObject.tag == "EntryPoints" || previousGoal.gameObject.tag == "SpawnPoints") {
                        goal = crowdManager.GoalRequest(previousGoal);
                    } else {
                        state = State.IDLE;
                    }
                }
            }    
        } 
        else if (state == State.IDLE) {
            agent.isStopped = true;
        }
    }

    public void SetGoal(Transform newGoal) {
        goal = newGoal;
    }
}
