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

    //Variables are set on awake to ensure they are instantiated without any null values
    void Awake() {
        animController = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        crowdManager = GameObject.Find("CrowdManager").GetComponent<AICrowdManager>();

        SetState(State.WALKING);
        animController.SetBool("Walking", true);
        
        // setting coroutines variables
        waitActive = false;
        switchState = false;

        //randomizing the navmesh
        float pSpeed = Random.Range(1.2f, 2.1f);
        float pStopDist = Random.Range (2.6f, 4.0f);
        agent.speed = pSpeed;
        agent.stoppingDistance = pStopDist;

        if ((state == State.WALKING) && (goal == null)) {
            goal = crowdManager.GoalRequest(previousGoal);
        }

        animController.SetBool("Walking", true);
    }

    // Update is called once per frame
    void Update()
    {
        //In walking state, the AI looks for a waypoint, reaches it and changes state to IDLE
        if(state == State.WALKING) {
            if (goal == null ) {
                goal = crowdManager.GoalRequest(previousGoal);    
            }
            walking = true;
            animController.SetBool("Walking", walking);
            agent.SetDestination(goal.transform.position);
            agent.isStopped = false;        
            
            if (agent.remainingDistance <= agent.stoppingDistance) {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                    state = State.IDLE;
                }
            }     
        } 
        //In idle state the AI choses one randoum animation and waits for n seconds. Then changes again to walking
        else if (state == State.IDLE) {
            agent.isStopped = true;
            walking = false;
            animController.SetBool("Walking", walking);
            animController.SetInteger("RandomIdle", RandomNumber());
            if (!crowdManager.IsSpawnReached(previousGoal)) {
                if (!waitActive) {
                    float timer = Random.Range(1.0f, 4.0f); 
                    StartCoroutine(IdlePause(timer));
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
            } else {

                crowdManager.RemovePeasant(this);
                Destroy(this.gameObject);

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
