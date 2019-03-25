using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICrowdManager : MonoBehaviour
{

    List<Transform> Waypoints = new List<Transform>();
    public List<Transform> SpawnPoints = new List<Transform>();
    public List<Transform> EntryPoints = new List<Transform>();
    public List<Peasant> Humans = new List<Peasant>();

    public GameObject humanPrefab;

    public int populationCount;

    // Start is called before the first frame update
    void Start()
    {
        GameObject waypointsHolder = GameObject.FindGameObjectWithTag("TrafficWaypoints");

        foreach (Transform child in waypointsHolder.transform) {
            Waypoints.Add(child);
            if (child.gameObject.tag.Equals("SpawnPoints")) {
                SpawnPoints.Add(child);
            } else if (child.gameObject.tag.Equals("EntryPoints")) {
                EntryPoints.Add(child);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Humans.Count <= populationCount) {
            Debug.Log("IN IF " + Humans.Count) ;
            //SpawnHuman();
            GameObject human = Instantiate(humanPrefab, SpawnPoints[RandomGenerator()].position, transform.rotation);
            Humans.Add(human.GetComponent<Peasant>());
            Humans[Humans.Count - 1].SetGoal(EntryPoints[RandomGenerator()]);
            Humans[Humans.Count - 1].previousGoal = SpawnPoints[0];
            Humans[Humans.Count - 1].SetState(Peasant.State.WALKING);

            
        }        
    }

    void SpawnHuman() {
        int randomStart = RandomGenerator();
        GameObject human = Instantiate(humanPrefab, SpawnPoints[randomStart].position, transform.rotation);
        //human.GetComponent<Peasant>().SetGoal(EntryPoints[randomStart]);
//        Humans.Add(human);
        
    }


    //The first and the last two elements of waypoints are entries or spawns. Remove the magic numbers (todo:)
    public Transform GoalRequest(Transform prevGoal) {
        if (SpawnPoints.Contains(prevGoal) || EntryPoints.Contains(prevGoal)) {
            return Waypoints[Random.Range(2, Waypoints.Count - 3)];

        }
        return Waypoints[Random.Range(0, Waypoints.Count-1)];
    }

    int RandomGenerator() {
        return Random.Range(0, 100) % 2; 
    }
}
