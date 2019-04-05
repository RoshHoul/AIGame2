using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class CharacterCinematic : MonoBehaviour
{
    [SerializeField]
    private bool inCinematic;
    private NavMeshAgent agent;
    [SerializeField]
    private GameObject father;
    private CharacterController charController;

    // Start is called before the first frame update
    void Awake()
    {
         
        agent = GetComponent<NavMeshAgent>();
        DisableFPC();
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(father.transform.position);
    //    DisableFPC();
    }

    private void DisableFPC() {
        GetComponent<FirstPersonController>().enabled = false;
    }

    public void EnableFPC() {
        GetComponent<FirstPersonController>().enabled = true;
        GetComponent<NavMeshAgent>().enabled = false;
    }

}
