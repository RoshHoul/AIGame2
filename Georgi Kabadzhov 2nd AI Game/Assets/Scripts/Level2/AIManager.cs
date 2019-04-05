using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject squad;

    bool waitActive, spawnNew = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!waitActive) {
            StartCoroutine(SpawnTimer(7f));
        }

        if (spawnNew) {
            Instantiate(squad, transform.position, transform.rotation);
            spawnNew = false;
        }
    }

    private IEnumerator SpawnTimer(float timer) {
        waitActive = true;
        yield return new WaitForSeconds(timer);
        spawnNew = true;
        waitActive = false;
    }
}
