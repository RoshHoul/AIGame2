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

    bool walking, waitActive, switchState;

    Animator animController;
    public enum State {
        IDLE,
        WALKING,
    }

    public State state;    

    void Awake() {
        animController = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        crowdManager = GameObject.Find("CrowdManager").GetComponent<AICrowdManager>();

        state = State.IDLE;
        Debug.Log("In start");
        
        waitActive = false;
        switchState = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.WALKING) {
            if (goal == null && previousGoal != null) {
                goal = crowdManager.GoalRequest(previousGoal);    
            }
            walking = true;
            animController.SetBool("Walking", walking);
            agent.SetDestination(goal.transform.position);
            agent.isStopped = false;        
            
            if (agent.remainingDistance <= agent.stoppingDistance) {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                    previousGoal = goal;
                    if (previousGoal.gameObject.tag == "EntryPoints") { 
                        goal = crowdManager.GoalRequest(previousGoal);
                    } else {
                        state = State.IDLE;
                    }
                }
            }     
        } 
        else if (state == State.IDLE) {
            agent.isStopped = true;
            walking = false;
            animController.SetBool("Walking", walking);
            animController.SetInteger("RandomIdle", RandomNumber());
            if (!waitActive) {
                StartCoroutine(IdlePause(3f));
            }
            if (switchState) {
                agent.isStopped = false;
                walking = true;
                animController.SetBool("Walking", walking);
                state = State.WALKING;
                previousGoal = goal;
                goal = crowdManager.GoalRequest(previousGoal);

                switchState = false;
            }


            if (agent.remainingDistance > agent.stoppingDistance) {
          //     state = State.WALKING; 
            }

        }
    }

    public void SetGoal(Transform newGoal) {
        goal = newGoal;
    }

    public void SetState(State newState) {
        Debug.Log("What?");
        state = newState;
        Debug.Log(state);
    }

    private IEnumerator IdlePause(float pauseTime) {
        waitActive = true;
        yield return new WaitForSeconds(pauseTime);
        switchState = true;
        waitActive = false;
    }

    private int RandomNumber() {
        return Random.Range(1,10) % 2;
    }

    
}
