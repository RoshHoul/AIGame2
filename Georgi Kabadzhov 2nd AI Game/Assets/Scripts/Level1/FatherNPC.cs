using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Simple AI to follow preset path
public class FatherNPC : MonoBehaviour
{

    public Transform[] waypoints;    

    [SerializeField]
    private CharacterCinematic cinemControl;


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
        //
        if (counter == 1) {
            StartCoroutine(TextPause());
            cinemControl.EnableFPC();
        }
    }

    IEnumerator TextPause() {
        yield return new WaitForSeconds(3f);
    }
}
