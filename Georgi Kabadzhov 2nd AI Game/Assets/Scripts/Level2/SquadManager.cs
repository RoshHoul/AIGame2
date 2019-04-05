using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Class for managing a squad of units
public class SquadManager : MonoBehaviour
{
    public int numUnits;
    public List<GameObject> UnitGoals = new List<GameObject>();
    public List<GameObject> Units = new List<GameObject>();

    public bool[] positionMap = new bool[16];
    private NavMeshAgent agent;
    public Transform goal; 

    public GameObject unitPrefab;
    public enum Formation { Square, Triangle, Break};
    public Formation formation;
    private bool waitActive, performCheck = false;

    public AnimationCurve critical;
    public AnimationCurve healthy;

    private float criticalValue = 0f;
    private float healthyValue = 0f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject tempObj = new GameObject();
        tempObj.transform.parent = gameObject.transform;
        goal = tempObj.transform;


        foreach (Transform child in transform) {
            if (child.gameObject.tag == "SquadMicroGoal") {
                UnitGoals.Add(child.gameObject);
            }
        }

        for (int i = 0; i < numUnits; i++) {
            positionMap[i] = false;
        }


        for (int i = 0; i < numUnits; i++) {
            GameObject unit = Instantiate(unitPrefab, UnitGoals[i].transform.position, transform.rotation);
            unit.transform.parent = gameObject.transform;
            Units.Add(unit);
            
        }


        GameObject wallTemp = GameObject.FindGameObjectWithTag("Wall");
        //goal = wallTemp.transform;
        float goalAreaX = wallTemp.GetComponent<Renderer>().bounds.size.x / 2;
        Vector3 center = wallTemp.GetComponent<Renderer>().bounds.center;
        float goalX = center.x + Random.Range(-goalAreaX/2, goalAreaX/2);
        float goalZ = wallTemp.transform.position.z;
        goal.position = new Vector3(goalX, 0, goalZ + 5);

        agent.SetDestination(goal.position);

    }


    public void EvaluateStatements() {
        int aliveUnits = Units.Count;

        healthyValue = healthy.Evaluate(aliveUnits);
        criticalValue = critical.Evaluate(aliveUnits);

        UpdateFormation();
    }

    private void UpdateFormation() {
        float decision = Random.Range(0f, 100f);
        float rangeMin = 0f;    
        float rangeMax = 100f;

        if (Mathf.Round(criticalValue * 100.0f)/100.0f > 0 ) {
            rangeMax = criticalValue * 100.0f;
            if (decision > rangeMin && decision < rangeMax) {
                formation = Formation.Break;
                
            } else {
                //Keep Attacking / Regroup
                //formation = Triangle/Square
            }
            ExecuteFormation();
        }
    } 


    //TODO:
    private void ExecuteFormation() {
        if (formation == Formation.Square) {
            for ( int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                   //GoalUnits = vec3(.., .., ..) 
                }
            }
        }

        if (formation == Formation.Break) {
            foreach (GameObject unit in Units) {
                unit.GetComponent<Unit>().Panic();
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

    public void DeadUnit(GameObject deadUnit) {
        Units.Remove(deadUnit);
    }

    //Fuzzy logic

    private IEnumerator SquadCheck(float timer) {
        waitActive = true;
        yield return new WaitForSeconds(timer);
        performCheck = true;
        waitActive = false;

    }
}
