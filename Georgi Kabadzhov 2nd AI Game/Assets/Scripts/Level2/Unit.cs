using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
 
//A class to manage a single unit
public class Unit : MonoBehaviour
{
    public SquadManager sqManager;

    [SerializeField]
    private Transform goal;
    private NavMeshAgent agent;

    public enum State { Attacking, Running, Dead}

    public State state;
    
    private bool waitActive, destroy = false;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Attacking;
        agent = GetComponent<NavMeshAgent>();
        sqManager = transform.parent.gameObject.GetComponent<SquadManager>();
        goal = sqManager.GoalRequest();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Attacking) {
            if (agent != null && goal != null) {
                agent.SetDestination(goal.position);
            }
        } else if (state == State.Dead) {
            if (!waitActive) {
                StartCoroutine(DeathTimer(4.0f));
            }

            if (destroy) {
                Destroy(gameObject);
            }
        }
    }

    public void UpdatePosition() {
        Transform tempLocation = sqManager.GoalRequest();
        if (tempLocation != null) {
            goal = tempLocation;
        }
    }

    public void Panic() {
        transform.parent = null;
        goal.position = new Vector3(Random.Range(0, 50), 0, Random.Range(400,500));
        agent.SetDestination(goal.position);
        agent.speed = 5;
        //make goal random position in the mountain
    }

    public void Die() {
        GetComponent<Animator>().enabled = false;
        agent.enabled = false;
        state = State.Dead;
        
        sqManager.DeadUnit(gameObject);
        sqManager.EvaluateStatements();
        transform.parent = null;

        
    }

    public IEnumerator DeathTimer(float timer) {
        waitActive = true;
        yield return new WaitForSeconds(timer);
        destroy = true;
        waitActive = false;
            
    }

}
