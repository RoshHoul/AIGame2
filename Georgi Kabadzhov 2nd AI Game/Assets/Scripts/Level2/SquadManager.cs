using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SquadManager : MonoBehaviour
{
    public int numUnits;
    public List<GameObject> UnitGoals = new List<GameObject>();
    public List<GameObject> Units = new List<GameObject>();

    public bool[] positionMap = new bool[16];
    private NavMeshAgent agent;
    public Transform goal; 

    public GameObject unitPrefab;

    public enum Formation { Square, Triangle};

    public Formation formation;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject tempObj = new GameObject();
        tempObj.transform.parent = gameObject.transform;
        goal = tempObj.transform;

        int debugCounter = 0;

        foreach (Transform child in transform) {
            if (child.gameObject.tag == "SquadMicroGoal") {
                Debug.Log(debugCounter);
                debugCounter++;
                UnitGoals.Add(child.gameObject);
            }
        }

/*      for (int i = 0; i < numUnits; i++) {
            GameObject tempUnitGoal = new GameObject();
            tempUnitGoal.transform.parent = gameObject.transform;
            UnitGoals.Add(tempUnitGoal);
        }
*/
        for (int i = 0; i < numUnits; i++) {
            positionMap[i] = false;
        }


        for (int i = 0; i < numUnits; i++) {
            Debug.Log("in instanciation: " + i);
            GameObject unit = Instantiate(unitPrefab, UnitGoals[i].transform.position, transform.rotation);
            unit.transform.parent = gameObject.transform;
            Units.Add(unit);
            
        }


        GameObject wallTemp = GameObject.FindGameObjectWithTag("Wall");
        //goal = wallTemp.transform;
        float goalAreaX = wallTemp.GetComponent<Renderer>().bounds.size.x / 2;
        Vector3 center = wallTemp.GetComponent<Renderer>().bounds.center;
        float goalX = center.x + Random.Range(-goalAreaX, goalAreaX);
        float goalZ = wallTemp.transform.position.z;
        goal.position = new Vector3(goalX, 0, goalZ);

        agent.SetDestination(goal.position);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //TODO:
    private void SetFormation() {
        if (formation == Formation.Square) {
            for ( int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    
                }
            }
        }
    }

    public Transform GoalRequest() {
        for (int i = 0; i < positionMap.Length; i++) {
            //easier than read to !positionMap, checking if that goal is used by another unit right now
            if (positionMap[i] == false) {
                positionMap[i] = true;
                return UnitGoals[i].transform;
            } 
        }
        return null;
    }
}
