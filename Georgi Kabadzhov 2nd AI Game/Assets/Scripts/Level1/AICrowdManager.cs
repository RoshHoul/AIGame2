using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICrowdManager : MonoBehaviour
{

    List<Transform> Waypoints = new List<Transform>();
    public List<Transform> SpawnPoints = new List<Transform>();
    public List<Transform> EntryPoints = new List<Transform>();
    public List<Peasant> Humans = new List<Peasant>();

    public List<GameObject> humanPrefab = new List<GameObject>();

    public int populationCount;
    public bool waitActive, spawnNew;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject waypointsHolder = GameObject.FindGameObjectWithTag("TrafficWaypoints");
        GameObject[] tempHumans = GameObject.FindGameObjectsWithTag("Peasant");

        foreach (GameObject human in tempHumans) {
            Humans.Add(human.GetComponent<Peasant>());
        }

        Debug.Log("humans count " + Humans.Count);

        waitActive = false;
        spawnNew = false;

        foreach (Transform child in waypointsHolder.transform) {
            if (child.gameObject.tag.Equals("SpawnPoints")) {
                SpawnPoints.Add(child);
            } else if (child.gameObject.tag.Equals("EntryPoints")) {
                EntryPoints.Add(child);
            }
            Waypoints.Add(child);
        }
    }

    //Creating a new human every 0.3 seconds);
    // Update is called once per frame
    void Update()
    {
        if (!waitActive) {
            StartCoroutine(SpawnTimer(0.3f));
        }

        if (spawnNew) {
            if (Humans.Count <= populationCount) {
                //SpawnHuman();
                GameObject human = Instantiate(humanPrefab[RandomGenerator()], SpawnPoints[Random.Range(0, SpawnPoints.Count)].position, transform.rotation);
                Humans.Add(human.GetComponent<Peasant>());
                Humans[Humans.Count - 1].SetState(Peasant.State.WALKING);
            }            
            spawnNew = false;
        }        
    }

    //Requests for new targets by the individual AI
    public Transform GoalRequest(Transform prevGoal) {
        return Waypoints[Random.Range(0, Waypoints.Count-1)];
    }

    public bool IsSpawnReached(Transform goal) {
        if (SpawnPoints.Contains(goal)) {
            return true;
        }

        return false;
    }

    private int RandomGenerator() {
        return Random.Range(0, 100) % 2; 
    }

    private IEnumerator SpawnTimer(float timer) {
        waitActive = true;
        yield return new WaitForSeconds(timer);
        spawnNew = true;
        waitActive = false;
    }

    //Remove human from collections
    public void RemovePeasant(Peasant p) {
        if (Humans.Contains(p)) {
           Humans.Remove(p); 
        }
    }

}
