using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class CharacterCinematic : MonoBehaviour
{
    [SerializeField]
    private bool inCinematic;
    private FirstPersonController firstPerson;
    private NavMeshAgent agent;
    [SerializeField]
    private GameObject father;
    private CharacterController charController;

    // Start is called before the first frame update
    void Awake()
    {
        
        charController = GetComponent<CharacterController>();
        firstPerson = GetComponent<FirstPersonController>();        
        agent = GetComponent<NavMeshAgent>();
        DisableFPC();
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(father.transform.position);
        DisableFPC();
    }

    void DisableFPC() {
        firstPerson.enabled = false;
        
    }

}
