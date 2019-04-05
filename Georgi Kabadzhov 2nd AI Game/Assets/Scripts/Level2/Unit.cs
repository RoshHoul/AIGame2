﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
 

public class Unit : MonoBehaviour
{
    public SquadManager sqManager;

    [SerializeField]
    private Transform goal;
    private NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        sqManager = transform.parent.gameObject.GetComponent<SquadManager>();
        goal = sqManager.GoalRequest();
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(goal.position);
    }

    public void SetGoal(Transform newGoal) {
        //goal = newGoal;
       // agent.SetDestination(goal.position);
    }
}
